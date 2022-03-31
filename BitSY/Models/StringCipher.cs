using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BitSY.Models
{
    internal static class StringCipher
    {
        public static byte[] EncryptToBytes(string plainText, string password)
        {
            if (plainText is not {Length: > 0})
                throw new ArgumentNullException(nameof(plainText));
            if (password is not {Length: > 0})
                throw new ArgumentNullException(nameof(password));

            using var aesAlgorithm = Aes.Create();
            var passwordBytes = Encoding.Latin1.GetBytes(password);


            aesAlgorithm.Key = SHA256.Create().ComputeHash(passwordBytes);
            aesAlgorithm.IV = MD5.Create().ComputeHash(passwordBytes);

            aesAlgorithm.Mode = CipherMode.CBC;
            aesAlgorithm.Padding = PaddingMode.PKCS7;

            var encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();

            return encrypted;
        }

        public static string DecryptFromBytes(byte[] cipherText, string password)
        {
            if (cipherText is not {Length: > 0})
                throw new ArgumentNullException(nameof(cipherText));
            if (password is not {Length: > 0})
                throw new ArgumentNullException(nameof(password));


            using var aesAlgorithm = Aes.Create();
            var passwordBytes = Encoding.Latin1.GetBytes(password);


            aesAlgorithm.Key = SHA256.Create().ComputeHash(passwordBytes);
            aesAlgorithm.IV = MD5.Create().ComputeHash(passwordBytes);

            aesAlgorithm.Mode = CipherMode.CBC;
            aesAlgorithm.Padding = PaddingMode.PKCS7;

            var decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }
    }
}