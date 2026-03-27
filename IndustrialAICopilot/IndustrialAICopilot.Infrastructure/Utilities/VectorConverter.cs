using System.Runtime.InteropServices;

namespace IndustrialAICopilot.Infrastructure.Utilities
{
    /// <summary>
    /// 向量資料轉換工具
    /// </summary>
    public static class VectorConverter
    {
        /// <summary>
        /// 將 float[] 轉為 byte[]。
        /// </summary>
        public static byte[] ToBytes(float[] floats)
        {
            if (floats == null || floats.Length == 0)
                return Array.Empty<byte>();

            return MemoryMarshal.Cast<float, byte>(floats).ToArray();
        }

        /// <summary>
        /// 將 byte[] 轉為 float[]。
        /// </summary>
        public static float[] ToFloatArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return Array.Empty<float>();

            return MemoryMarshal.Cast<byte, float>(bytes).ToArray();
        }
    }
}
