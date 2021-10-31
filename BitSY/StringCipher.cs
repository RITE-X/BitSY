using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BitSY
{
    internal static class StringCipher
    {

        public static byte[] EncryptToBytes(string plainText, byte[] key, byte[] IV)
        {
            if (plainText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(plainText));
            if (key is not { Length: > 0 })
                throw new ArgumentNullException(nameof(key));
            if (IV is not { Length: > 0 })
                throw new ArgumentNullException(nameof(IV));
            byte[] encrypted;


            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

   
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

          
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static string DecryptFromBytes(byte[] cipherText, byte[] key, byte[] IV)
        {
            if (cipherText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(cipherText));
            if (key is not { Length: > 0 })
                throw new ArgumentNullException(nameof(key));
            if (IV is not { Length: > 0 })
                throw new ArgumentNullException(nameof(IV));


            string plaintext;


            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

          
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

               
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}