namespace IndustrialAICopilot.Infrastructure.Utilities
{
    /// <summary>
    /// 向量空間運算的輔助工具
    /// </summary>
    public static class VectorHelper
    {
        /// <summary>
        /// 計算兩個向量之間的餘弦相似度。
        /// 結果介於 -1 到 1 之間，越接近 1 代表語意越相似。
        /// </summary>
        public static float CosineSimilarity(float[] first, float[] second)
        {
            if (first.Length != second.Length)
                throw new ArgumentException("向量維度必須一致才能進行相似度計算。");

            float dotProduct = 0;
            float normFirst = 0;
            float normSecond = 0;

            for (int i = 0; i < first.Length; i++)
            {
                dotProduct += first[i] * second[i];
                normFirst += MathF.Pow(first[i], 2);
                normSecond += MathF.Pow(second[i], 2);
            }

            return dotProduct / (MathF.Sqrt(normFirst) * MathF.Sqrt(normSecond) + 1e-8f);
        }
    }
}
