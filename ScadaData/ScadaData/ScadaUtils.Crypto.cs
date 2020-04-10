using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Scada
{
    partial class ScadaUtils
    {
        /// <summary>
        /// Generates cryptographically strong random numbers.
        /// </summary>
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        /// <summary>
        /// The default secret key.
        /// </summary>
        private static readonly byte[] DefaultSecretKey = new byte[SecretKeySize] {
            0x0A, 0xBA, 0x06, 0xBC, 0x1A, 0x5D, 0x44, 0x3E, 0x5A, 0xE8, 0x46, 0x7F, 0xB8, 0x85, 0x49, 0xF6,
            0xE9, 0xCC, 0x90, 0xF0, 0x80, 0x45, 0x33, 0xFC, 0x2A, 0x67, 0xD9, 0xBA, 0x00, 0xCE, 0xC7, 0x8A };
        /// <summary>
        /// The default initialization vector.
        /// </summary>
        private static readonly byte[] DefaultIV = new byte[IVSize] {
            0xA5, 0x5C, 0x5A, 0x7B, 0x40, 0xD4, 0x2D, 0x33, 0xA4, 0x6F, 0xF7, 0x84, 0x94, 0x1C, 0x47, 0x85 };

        /// <summary>
        /// The secret key size in bytes.
        /// </summary>
        public const int SecretKeySize = 32;
        /// <summary>
        /// The initialization vector size in bytes.
        /// </summary>
        public const int IVSize = 16;


        /// <summary>
        /// Reads all data from the stream.
        /// </summary>
        private static byte[] ReadToEnd(Stream inputStream)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                inputStream.CopyTo(memStream);
                return memStream.ToArray();
            }
        }


        /// <summary>
        /// Gets random 64-bit integer.
        /// </summary>
        public static long GetRandomLong()
        {
            byte[] randomArr = new byte[8];
            Rng.GetBytes(randomArr);
            return BitConverter.ToInt64(randomArr, 0);
        }

        /// <summary>
        /// Gets random byte array.
        /// </summary>
        public static byte[] GetRandomBytes(int count)
        {
            byte[] randomArr = new byte[count];
            Rng.GetBytes(randomArr);
            return randomArr;
        }

        /// <summary>
        /// Computes the MD5 hash value for the specified byte array.
        /// </summary>
        public static string ComputeHash(byte[] bytes)
        {
            return BytesToHex(MD5.Create().ComputeHash(bytes));
        }

        /// <summary>
        /// Computes the MD5 hash value for the specified string.
        /// </summary>
        public static string ComputeHash(string s)
        {
            return ComputeHash(Encoding.UTF8.GetBytes(s));
        }

        /// <summary>
        /// Encrypts the string with the specified secret key and initialization vector.
        /// </summary>
        public static string Encrypt(string s, byte[] secretKey, byte[] iv)
        {
            return BytesToHex(EncryptBytes(Encoding.UTF8.GetBytes(s), secretKey, iv));
        }

        /// <summary>
        /// Encrypts the string with the default secret key and initialization vector.
        /// </summary>
        public static string Encrypt(string s)
        {
            return Encrypt(s, DefaultSecretKey, DefaultIV);
        }

        /// <summary>
        /// Encrypts the byte array.
        /// </summary>
        public static byte[] EncryptBytes(byte[] bytes, byte[] secretKey, byte[] iv)
        {
            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = iv };

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateEncryptor(secretKey, iv), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }

                    return memStream.ToArray();
                }
            }
            finally
            {
                alg?.Clear();
            }
        }

        /// <summary>
        /// Decrypts the string with the specified secret key and initialization vector.
        /// </summary>
        public static string Decrypt(string s, byte[] secretKey, byte[] iv)
        {
            return string.IsNullOrEmpty(s) ? "" : Encoding.UTF8.GetString(DecryptBytes(HexToBytes(s), secretKey, iv));
        }

        /// <summary>
        /// Decrypts the string with the default secret key and initialization vector.
        /// </summary>
        public static string Decrypt(string s)
        {
            return Decrypt(s, DefaultSecretKey, DefaultIV);
        }

        /// <summary>
        /// Decrypts the byte array.
        /// </summary>
        public static byte[] DecryptBytes(byte[] bytes, byte[] secretKey, byte[] iv)
        {
            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = iv };

                using (MemoryStream memStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateDecryptor(secretKey, iv), CryptoStreamMode.Read))
                    {
                        return ReadToEnd(cryptoStream);
                    }
                }
            }
            finally
            {
                alg?.Clear();
            }
        }
    }
}
