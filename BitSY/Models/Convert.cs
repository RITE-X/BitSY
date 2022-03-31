using System;
using System.Collections;

namespace BitSY.Models
{
    internal static class Convert
    {
        public static byte BitsToByte(BitArray item)
        {
            byte num = 0;
            for (var i = 0; i < item.Count; i++)
                if (item[i])
                {
                    num += (byte)Math.Pow(2, i);
                }

            return num;
        }

        public static BitArray ByteToBits(byte item)
        {
            var bitArray = new BitArray(8);

          
            for (var i = 0; i < bitArray.Length; i++)
            {
                var bit = (item >> i & 1) == 1;
                bitArray[i] = bit;
            }

            return bitArray;
        }

        public static bool[] BytesToBits(byte[] bytes)
        {
            var textInBits = new bool[bytes.Length * 8];

            var iterator = 0;
            foreach (var @byte in bytes)
            {
                var character = ByteToBits(@byte);
                for (var j = 0; j < 8; j++)
                {
                    textInBits[iterator] = character[j];
                    iterator++;
                }
            }

            return textInBits;
        }

        public static byte[] BitsToBytes(bool[] bits)
        {
            var bytes = new byte[bits.Length / 8];


            var iterator = 0;
            for (var i = 0; i < bytes.Length; i++)
            {
                var nextByte = new BitArray(8);
                for (var j = 0; j < 8; j++)
                {
                    nextByte[j] = bits[iterator]; 
                    iterator++;
                }

                bytes[i] = BitsToByte(nextByte);
            }

            return bytes;
        }

        public static int BytesToInt(byte[] bytes)
        {
            if (bytes.Length > 4) throw new ArgumentOutOfRangeException(nameof(bytes));
            return BitConverter.ToInt32(bytes);
        }

        public static byte[] IntToBytes(int integer)
        {
            return BitConverter.GetBytes(integer);
        }

        public static string BytesToString(byte[] content)
        {
            var str = new char[content.Length];
            for (var i = 0; i < str.Length; i++)
            {
                str[i] = (char)content[i];
            }

            return new string(str);
        }

        public static byte[] StringToBytes(string str)
        {
            var content = new byte[str.Length];
            for (var i = 0; i < str.Length; i++)
            {
                content[i] = (byte)str[i];
            }

            return content;
        }
    }
}