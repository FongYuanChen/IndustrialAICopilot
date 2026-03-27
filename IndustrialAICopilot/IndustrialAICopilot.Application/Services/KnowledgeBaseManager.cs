using IndustrialAICopilot.Application.Interfaces;
using IndustrialAICopilot.Application.Models;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Application.Services
{
    public class KnowledgeBaseManager : IKnowledgeBaseManager
    {
        private readonly IAISettingsManager _aiSettingsManager;
        private readonly IKnowledgeBaseRepository _knowledgeBaseRepository;
        private readonly IEnumerable<ITextExtractor> _textExtractors;
        private readonly IEnumerable<ITextSplitter> _textSplitters;
        private readonly IEnumerable<ITextEmbedder> _textEmbedders;

        private AISettingsContext _currentSettingsContext;

        private List<Document> _documentCache;
        private List<DocumentChunk> _documentChunkCache;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public KnowledgeBaseManager(IAISettingsManager aiSettingsManager,
                                    IKnowledgeBaseRepository knowledgeBaseRepository,
                                    IEnumerable<ITextExtractor> textExtractors,
                                    IEnumerable<ITextSplitter> textSplitters,
                                    IEnumerable<ITextEmbedder> textEmbedders)
        {
            _aiSettingsManager = aiSettingsManager;
            _aiSettingsManager.OnContextChanged += async (newSettingsContext) =>
            {
                try
                {
                    var oldSettingsContext = _currentSettingsContext ?? new AISettingsContext();

                    _currentSettingsContext = newSettingsContext;

                    if (newSettingsContext.AISettingsIsValid &&
                        newSettingsContext.AISettings.EmbeddingModelName != oldSettingsContext.AISettings?.EmbeddingModelName)
                    {
                        Console.WriteLine($"開始自動重建索引...");
                        await RebuildDocumentChunkEmbeddingsAsync();
                        Console.WriteLine($"自動重建索引完成!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"自動重建索引失敗: {ex}");
                }
            };
            _knowledgeBaseRepository = knowledgeBaseRepository;
            _textExtractors = textExtractors;
            _textSplitters = textSplitters;
            _textEmbedders = textEmbedders;
        }

        /// <summary>
        /// 獲取目前系統支援處理的文件副檔名清單。
        /// </summary>
        public Task<List<string>> GetSupportedExtensionsAsync()
        {
            var extensions = _textExtractors
                .SelectMany(extractor => extractor.SupportedExtensions)
                .Select(extractor => extractor.ToLowerInvariant())
                .Distinct()
                .ToList();

            return Task.FromResult(extensions);
        }

        /// <summary>
        /// 非同步獲取目前知識庫中所有的文件資訊。
        /// </summary>
        public async Task<List<Document>> GetDocumentsAsync()
        {
            if (_documentCache == null)
            {
                await _lock.WaitAsync();
                try
                {
                    if (_documentCache == null)
                    {
                        await ReloadCacheAsync();
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            return _documentCache != null ? new List<Document>(_documentCache) : new List<Document>();
        }

        /// <summary>
        /// 非同步獲取目前知識庫中所有的文件區塊資訊。
        /// </summary>
        public async Task<List<DocumentChunk>> GetDocumentChunksAsync()
        {
            if (_documentChunkCache == null)
            {
                await _lock.WaitAsync();
                try
                {
                    if (_documentChunkCache == null)
                    {
                        await ReloadCacheAsync();
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            return _documentChunkCache != null ? new List<DocumentChunk>(_documentChunkCache) : new List<DocumentChunk>();
        }

        /// <summary>
        /// 非同步匯入新文件，包含內容提取、文字切分、向量轉化與存儲。
        /// </summary>
        public async Task<(Document Document, List<DocumentChunk> Chunks)> IngestDocumentAsync(string name, long size, Stream stream)
        {
            var (document, documentChunks) = await PrepareDocumentDataAsync(name, size, stream);

            await _lock.WaitAsync();
            try
            {
                await _knowledgeBaseRepository.InsertDocumentAsync(document, documentChunks);

                AppendToCache(document, documentChunks);

                return (document, documentChunks);
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 非同步重建知識庫，重新計算所有現存文件的向量資訊。
        /// </summary>
        public async Task RebuildDocumentChunkEmbeddingsAsync()
        {
            var newEmbeddings = await PrepareDocumentChunkEmbeddingDatumAsync();

            await _lock.WaitAsync();
            try
            {
                await _knowledgeBaseRepository.UpdateDocumentChunkEmbeddingAsync(newEmbeddings);

                await ReloadCacheAsync();
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 非同步刪除指定文件及其關聯的所有資訊。
        /// </summary>
        public async Task DeleteDocumentAsync(Guid documentId)
        {
            await _lock.WaitAsync();
            try
            {
                await _knowledgeBaseRepository.DeleteDocumentAsync(documentId);
                await ReloadCacheAsync();
            }
            finally
            {
                _lock.Release();
            }
        }


        #region 私有方法

        private async Task<(Document Document, List<DocumentChunk> Chunks)> PrepareDocumentDataAsync(string name, long size, Stream stream)
        {
            var settingsContext = await GetAISettingsContextAsync();
            if (!settingsContext.AISettingsIsValid)
                throw new InvalidOperationException("無效的 AI 配置資訊。請重新設定。");
            var settings = settingsContext.AISettings;
            var textExtractor = _textExtractors.FirstOrDefault(extractor => extractor.CanHandle(settings) && extractor.CanHandle(Path.GetExtension(name)))
                ?? throw new NotSupportedException($"找不到適合目前配置的文字解析工具。");
            var textSplitter = _textSplitters.FirstOrDefault(splitter => splitter.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的文字切分工具。");
            var textEmbedder = _textEmbedders.FirstOrDefault(embedder => embedder.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的文字向量化工具。");

            var documentId = Guid.NewGuid();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var rawText = await textExtractor.ExtractAsync(memoryStream, settings);
            var chunks = await textSplitter.Split(rawText, settings);
            var documentChunkTasks = chunks.Select(async chunk =>
            {
                var vector = await textEmbedder.EmbedAsync(chunk, settings);
                return new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentId = documentId,
                    Content = chunk,
                    Vector = vector
                };
            });
            var documentChunks = (await Task.WhenAll(documentChunkTasks)).ToList();
            var document = new Document
            {
                Id = documentId,
                Name = name,
                Size = size,
                CreatedAt = DateTime.Now
            };
            return (document, documentChunks);
        }

        private async Task<(Guid ChunkId, float[] Vector)[]> PrepareDocumentChunkEmbeddingDatumAsync()
        {
            var settingsContext = await GetAISettingsContextAsync();
            if (!settingsContext.AISettingsIsValid)
                throw new InvalidOperationException("無效的 AI 配置資訊。請重新設定。");
            var settings = settingsContext.AISettings;
            var textEmbedder = _textEmbedders.FirstOrDefault(e => e.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的文字向量化工具。");

            var documentChunks = await _knowledgeBaseRepository.GetDocumentChunksAsync();
            var newEmbeddingTasks = documentChunks.Select(async chunk =>
            {
                var vector = await textEmbedder.EmbedAsync(chunk.Content, settings);
                return (chunk.Id, vector);
            });
            return await Task.WhenAll(newEmbeddingTasks);
        }

        private async Task<AISettingsContext> GetAISettingsContextAsync()
        {
            if (_currentSettingsContext?.AISettings == null)
            {
                _currentSettingsContext = await _aiSettingsManager.GetContextAsync();
            }
            return _currentSettingsContext;
        }

        private void AppendToCache(Document document, List<DocumentChunk> chunks)
        {
            // 只有在快取已存在(已被載入過)時才執行附加，否則等下一次 Get 時自然會載入
            _documentCache?.Add(document);
            _documentChunkCache?.AddRange(chunks);
        }

        private async Task ReloadCacheAsync()
        {
            _documentCache = (await _knowledgeBaseRepository.GetDocumentsAsync()).ToList();
            _documentChunkCache = (await _knowledgeBaseRepository.GetDocumentChunksAsync()).ToList();
        }

        #endregion
    }
}
