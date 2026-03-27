using IndustrialAICopilot.Core.Models;
using LangChain.Splitters.Text;

namespace IndustrialAICopilot.Infrastructure.TextSplitters
{
    public class LangChainRecursiveTextSplitter : Core.Interfaces.ITextSplitter
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings) => true;

        /// <summary>
        /// 非同步切分流程，將文字分割為字串陣列。
        /// </summary>
        public async Task<string[]> Split(string text, AISettings settings)
        {
            var textSplitter = new RecursiveCharacterTextSplitter();
            var textChunks = textSplitter.SplitText(text);
            return textChunks.ToArray();
        }
    }
}
