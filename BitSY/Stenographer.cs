namespace BitSY
{
    internal static class Stenographer
    {
        public static void WriteBitsInBitmap(bool[] content, ref byte[] bitmap)
        {
            var iterator = 0;
            for (var i = 58; i < bitmap.Length && iterator != content.Length; i++)
            {
                var color = Convert.ByteToBits(bitmap[i]);
                for (var j = 0; j < 2; j++)
                {
                    color[j] = content[iterator];
                    iterator++;
                }

                bitmap[i] = Convert.BitsToByte(color);
            }
        }

        public static bool[] ReadBitsFromBitmap(byte[] bitmap, int count)
        {
            var myWord = new bool[count];
            var iterator = 0;
            for (var i = 58; i < count / 2 + 58; i++)
            {
                var color = Convert.ByteToBits(bitmap[i]);
                for (var j = 0; j < 2; j++)
                {
                    myWord[iterator] = color[j];
                    iterator++;
                }
            }

            return myWord;
        }

        public static byte[] WriteTextLenghtToKey(byte[] key, int lenght)
        {
            var newKey = (byte[])key.Clone();
            var lenghtInBytes = Convert.IntToBytes(lenght);

            for (int i = key.Length - lenghtInBytes.Length, j = 0; i < key.Length; i++, j++)
            {
                newKey[i] = lenghtInBytes[j];
            }

            return newKey;
        }

        public static int ReadTextLenghtFromKey(byte[] key)
        {
            var lenghtInBytes = new byte[4];
            for (int i = key.Length - 4, j = 0; i < key.Length; i++, j++)
            {
                lenghtInBytes[j] = key[i];
            }

            return Convert.BytesToInt(lenghtInBytes);
        }

        public static void WriteTextLenghtToBitmap(ref byte[] bitmap, int lenght)
        {
            var lenghtInBytes = Convert.IntToBytes(lenght);

            for (int i = 54, j = 0; i < 58; i++, j++)
            {
                bitmap[i] = lenghtInBytes[j];
            }
        }

        public static int ReadTextLenghtFromBitmap(byte[] bitmap)
        {
            var lenghtInBytes = new byte[4];
            for (int i = 54, j = 0; i < 58; i++, j++)
            {
                lenghtInBytes[j] = bitmap[i];
            }

            return Convert.BytesToInt(lenghtInBytes);
        }
    }
}