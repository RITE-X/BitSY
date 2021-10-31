using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSY
{
    internal static class Stenographer
    {
        public static byte[] WriteBitesInBitmap(bool[] content, byte[] bitmap)
        {
            byte[] newTarget = (byte[])bitmap.Clone();

            var iterator = 0;
            for (var i = 54; i < newTarget.Length && iterator != content.Length; i++)
            {
                var color = Convert.ByteToBits(bitmap[i]);
                for (var j = color.Length - 2; j < color.Length; j++)
                {
                    color[j] = content[iterator];
                    iterator++;
                }

                newTarget[i] = Convert.BitsToByte(color);
            }

            return newTarget;
        }

        public static void ReadBitesFromBitmap(byte[] bitmap, int count)
        {

            var myWord = new bool[count];
            int iterator = 0;
            for (var i = 54; i < count / 2 + 54; i++)
            {
                var color = Convert.ByteToBits(bitmap[i]);
                for (var j = color.Length - 2; j < color.Length; j++)
                {
                    myWord[iterator] = color[j];
                    iterator++;
                }
            }
        }
    }
}
