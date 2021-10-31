using System;
using System.Collections;


namespace BitSY
{
    internal static class Convert
    {
        public static byte BitsToByte(BitArray scr)
        {
            byte num = 0;
            for (var i = 0; i < scr.Count; i++)
                if (scr[i] == true)
                    num += (byte)Math.Pow(2, i);
            return num;
        }

        public static BitArray ByteToBits(byte src)
        {
            var bitArray = new BitArray(8);
            for (var i = 0; i < bitArray.Length; i++)
            {
                var bit = (src >> i & 1) == 1;
                bitArray[i] = bit;
            }
            return bitArray;
        }

        public static bool[] BytesToBits(byte[] bytes)
        {
            var textInBites = new bool[bytes.Length * 8];

            var iterator = 0;
            foreach (var _byte in bytes)
            {
                var character = ByteToBits(_byte);
                for (var j = 0; j < 8; j++)
                {
                    textInBites[iterator] = character[j];
                    iterator++;
                }
            }

            return textInBites;
        }

        public static byte[] BitsToBytes(bool[] bites)
        {
            byte[] bytes = new byte[bites.Length/8];


            int g = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                var gf = new BitArray(8);
                for (int j = 0; j < 8; j++)
                {
                    gf[j] = bites[g];
                    g++;
                }

                bytes[i] = Convert.BitsToByte(gf);
            }
            return bytes;
        }

    }
}

