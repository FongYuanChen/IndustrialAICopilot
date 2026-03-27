using IndustrialAICopilot.Application.Interfaces;
using IndustrialAICopilot.Core.Interfaces;
using System.Text;

namespace IndustrialAICopilot.Application.Services
{
    public class QuestionResponder : IQuestionResponder
    {
        private readonly IAISettingsManager _aiSettingsManager;
        private readonly IKnowledgeBaseManager _knowledgeBaseManager;
        private readonly IEnumerable<ITextEmbedder> _textEmbedders;
        private readonly IEnumerable<ITextSearcher> _textSearchers;
        private readonly IEnumerable<IPromptGenerator> _promptGenerators;  
        private readonly IEnumerable<ITextGenerator> _textGenerators; 

        public QuestionResponder(IAISettingsManager aiSettingsManager,
                                 IKnowledgeBaseManager knowledgeBaseManager,
                                 IEnumerable<ITextEmbedder> textEmbedders,
                                 IEnumerable<ITextSearcher> textSearchers,
                                 IEnumerable<IPromptGenerator> promptGenerators,
                                 IEnumerable<ITextGenerator> textGenerators)
        {
            _aiSettingsManager = aiSettingsManager;
            _knowledgeBaseManager = knowledgeBaseManager;
            _textEmbedders = textEmbedders;
            _textSearchers = textSearchers;
            _promptGenerators = promptGenerators;
            _textGenerators = textGenerators;
        }
        /// <summary>
        /// 非同步執行完整的問題回應流程（包含提問向量化、知識庫語意比對、上下文組裝、AI 模型生成回覆）。
        /// </summary>
        public async Task<string> RespondAsync(string query)
        {
            var settingsContext = await _aiSettingsManager.GetContextAsync();
            if (!settingsContext.AISettingsIsValid)
                throw new InvalidOperationException("無效的 AI 配置資訊。請重新設定。");
            var settings = settingsContext.AISettings;
            var textEmbedder = _textEmbedders.FirstOrDefault(embedder => embedder.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的文字向量化工具。");
            var textSearcher = _textSearchers.FirstOrDefault(searcher => searcher.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的搜尋引擎。");
            var promptGenerator = _promptGenerators.FirstOrDefault(generator => generator.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的提示詞生成工具。");
            var textGenerator = _textGenerators.FirstOrDefault(generator => generator.CanHandle(settings))
                ?? throw new NotSupportedException("找不到適合目前配置的文字生成工具。");

            var documentChunks = await _knowledgeBaseManager.GetDocumentChunksAsync();
            var queryVector = await textEmbedder.EmbedAsync(query, settings);
            var searchResults = await textSearcher.SearchAsync(documentChunks.ToArray(), queryVector, 5, settings);
            var contextBuilder = new StringBuilder();
            foreach (var searchResult in searchResults)
            {
                contextBuilder.Append(searchResult);
                contextBuilder.AppendLine();
            }
            var context = contextBuilder.ToString();
            var prompt = await promptGenerator.GenerateAsync(query, context, settings);
            var answer = await textGenerator.GenerateAsync(prompt, settings);
            return answer;
        }
    }
}
