using System;
using BitSY.ViewModels.Base;

namespace BitSY.Models
{

    public sealed class BitmapStenographer : ViewModel
    {
        private readonly int _endOfMetadataImage;


        private byte[]? _bitmapSource;

        public byte[]? BitmapSource
        {
            get => _bitmapSource;
            set
            {
                _bitmapSource = value;
                BitmapEnd = _bitmapSource.Length;
            }
        }


        private long _bitmapEnd;

        public long BitmapEnd
        {
            get => BitmapSource?.Length - sizeof(int) ?? _endOfMetadataImage;
            set => Set(ref _bitmapEnd, value);
        }

        private long _textLengthPosition;

        public long TextLengthPosition
        {
            get => _textLengthPosition;
            set => Set(ref _textLengthPosition, value);
        }


        private int _writeBitCount;

        public int WriteBitCount
        {
            get => _writeBitCount;
            set => Set(ref _writeBitCount, value);
        }


        public BitmapStenographer(int endOfMetadata)
        {
            if (endOfMetadata <= 0) throw new ArgumentOutOfRangeException(nameof(endOfMetadata));
            _endOfMetadataImage = endOfMetadata;

            _textLengthPosition = endOfMetadata; //default value 
            _writeBitCount = 1; //default value 
        }

        
        public void WriteBitsInBitmap(bool[] content)
        {
            var iterator = 0;
            for (var i = _endOfMetadataImage; i < BitmapEnd && iterator != content.Length; i++)
            {
                if (i >= TextLengthPosition && i < TextLengthPosition + sizeof(int))
                    continue;


                var color = Convert.ByteToBits(BitmapSource[i]);
                for (var j = 0; j < _writeBitCount; j++)
                {
                    color[j] = content[iterator];
                    iterator++;
                }

                BitmapSource[i] = Convert.BitsToByte(color);
            }
        }

        public bool[] ReadBitsFromBitmap(int count)
        {
            var readRange = count / _writeBitCount + _endOfMetadataImage;
            if (readRange > BitmapEnd)
                throw new ArgumentOutOfRangeException(paramName: nameof(count));

            var myWord = new bool[count];
            var iterator = 0;


            for (var i = _endOfMetadataImage; i < readRange; i++)
            {
                if (i >= TextLengthPosition && i < TextLengthPosition + sizeof(int))
                {
                    readRange++;
                    continue;
                }


                var color = Convert.ByteToBits(BitmapSource[i]);
                for (var j = 0; j < _writeBitCount; j++)
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

            for (long i = TextLengthPosition, j = 0; i < TextLengthPosition + sizeof(int); i++, j++)
            {
                BitmapSource[i] = lenghtInBytes[j];
            }
        }

        public int ReadTextLenghtFromBitmap()
        {
            var lenghtInBytes = new byte[sizeof(int)];
            for (long i = TextLengthPosition, j = 0; i < TextLengthPosition + sizeof(int); i++, j++)
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