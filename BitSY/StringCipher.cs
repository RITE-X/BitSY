using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BitSY
{
    internal static class StringCipher
    {
        public static byte[] EncryptToBytes(string plainText, string password)
        {
            if (plainText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(plainText));
            if (password is not { Length: > 0 })
                throw new ArgumentNullException(nameof(password));

            byte[] encrypted;

            using (var aesAlgorim = Aes.Create())
            {
                var passwordBytes = Encoding.Latin1.GetBytes(password);

                aesAlgorim.Key = SHA256.Create().ComputeHash(passwordBytes);
                aesAlgorim.IV = MD5.Create().ComputeHash(passwordBytes);

                aesAlgorim.Mode = CipherMode.CBC;
                aesAlgorim.Padding = PaddingMode.PKCS7;

                var encryptor = aesAlgorim.CreateEncryptor(aesAlgorim.Key, aesAlgorim.IV);

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

        public static string DecryptFromBytes(byte[] cipherText, string password)
        {
            if (cipherText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(cipherText));
            if (password is not { Length: > 0 })
                throw new ArgumentNullException(nameof(password));


            string plaintext;

            using (var aesAlgorim = Aes.Create())
            {
                var passwordBytes = Encoding.Latin1.GetBytes(password);


                aesAlgorim.Key = SHA256.Create().ComputeHash(passwordBytes);
                aesAlgorim.IV = MD5.Create().ComputeHash(passwordBytes);

                aesAlgorim.Mode = CipherMode.CBC;
                aesAlgorim.Padding = PaddingMode.PKCS7;

                var decryptor = aesAlgorim.CreateDecryptor(aesAlgorim.Key, aesAlgorim.IV);

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