using System;
using BitSY.ViewModels.Base;

namespace BitSY.Models
{

    public sealed class BitmapStenographer : ViewModel
    {
        private readonly int _endOfMetadataImage;


        public byte[]? BitmapSource { get; set; }


        private int _writeBitCount;


        public BitmapStenographer(int endOfMetadata)
        {
            if (endOfMetadata <= 0) throw new ArgumentOutOfRangeException(nameof(endOfMetadata));
            _endOfMetadataImage = endOfMetadata;
            
            _writeBitCount = 2; //default value 
        }

        
        public void WriteBitsInBitmap(bool[] content)
        {
            var iterator = 0;
            for (var i = 58; i < BitmapSource.Length && iterator != content.Length; i++)
            {
                var color = Convert.ByteToBits(BitmapSource[i]);
                for (var j = 0; j < 2; j++)
                {
                    color[j] = content[iterator];
                    iterator++;
                }

                BitmapSource[i] = Convert.BitsToByte(color);
            }
        }

        public bool[] ReadBitsFromBitmap(int count)
        {
            var myWord = new bool[count];
            var iterator = 0;
            for (var i = 58; i < count / 2 + 58; i++)
            {
                var color = Convert.ByteToBits(BitmapSource[i]);
                for (var j = 0; j < 2; j++)
                {
                    myWord[iterator] = color[j];
                    iterator++;
                }
            }

            return myWord;
        }

        public void WriteTextLenghtToBitmap(int lenght)
        {
            var lenghtInBytes = Convert.IntToBytes(lenght);

            for (int i = 54, j = 0; i < 58; i++, j++)
            {
                BitmapSource[i] = lenghtInBytes[j];
            }
        }

        public int ReadTextLenghtFromBitmap()
        {
            var lenghtInBytes = new byte[4];
            for (int i = 54, j = 0; i < 58; i++, j++)
            {
                lenghtInBytes[j] = BitmapSource[i];
            }

            return Convert.BytesToInt(lenghtInBytes);
        }


        public static byte[] WriteTextLenghtToKey(int lenght, byte[] key)
        {
            var newKey = (byte[]) key.Clone();
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
    }
}