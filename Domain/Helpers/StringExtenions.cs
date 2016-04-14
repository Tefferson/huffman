using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string ToByteString(this string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BytesToString(this string data)
        {
            IList<byte> byteList = new List<byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray()).RemoveLast();
        }

        public static string RemoveLast(this string data)
        {
            return data.Substring(0, data.Length - 1);
        }

        private static bool[] ToBinaryArray(string strData)
        {
            byte[] data = strData.ToCharArray().Select(c => (byte)c).ToArray();
            return data.Select(c => c == '1').ToArray();
        }

        public static BitArray ToBitArray(this string data)
        {
            return new BitArray(ToBinaryArray(data));
        }
    }
}
