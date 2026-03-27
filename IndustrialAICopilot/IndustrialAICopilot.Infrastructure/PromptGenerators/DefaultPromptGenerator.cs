using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Infrastructure.PromptGenerators
{
    public class DefaultPromptGenerator : IPromptGenerator
    {
        private const string _variableContext = "{context}";
        private const string _variableQuery = "{query}";
        private const string _defaultTemplate =
            $@"
            你是一位專業的知識庫助手。請根據下方提供的 [知識庫資料] 來回答 [使用者提問]。

            [知識庫資料]
            {_variableContext}

            [使用者提問]
            {_variableQuery}

            [指示]
            1. 僅根據上方提供的 [知識庫資料] 內容進行回答，不可引用外部知識。
            2. 回答內容須精簡、準確，並直接針對問題。
            3. 若 [知識庫資料] 中不包含答案所需的資訊，請直接回覆：「目前知識庫中無相關參考資料可供回答。」
            4. 必須使用與 [使用者提問] 相同的語言進行回覆。
            ";

        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings) => true;

        /// <summary>
        /// 非同步生成流程，根據使用者提問與知識庫檢索結果，依照樣板產出提示指令。
        /// </summary>
        public async Task<string> GenerateAsync(string query, string context, AISettings settings)
        {
            return _defaultTemplate.Replace(_variableQuery, query ?? string.Empty).Replace(_variableContext, context ?? string.Empty);
        }
    }
}
