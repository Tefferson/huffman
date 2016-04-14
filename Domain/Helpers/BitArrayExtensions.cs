using System.Collections;
using System.Linq;
using System.Text;

namespace System
{
    public static class BitArrayExtensions
    {
        public static string ToDigitString(this BitArray bits)
        {
            StringBuilder sb = new StringBuilder();
            foreach (bool bit in bits.Cast<bool>())
            {
                sb.Append(bit ? "1" : "0");
            }
            return sb.ToString();
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    bytes[byteIndex] |= (byte)(1 << (bitIndex));
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }
    }
}
