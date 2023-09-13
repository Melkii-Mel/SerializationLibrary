using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;

namespace Serialization.src.Local
{
    internal class Encrypter
    {
        private byte[] DefaultKey => new byte[16]
            {
                0x02, 0x04, 0x05, 0x04, 0x01, 0x01, 0x12, 0x02,
                0x01, 0x11, 0x15, 0x12, 0x15, 0x11, 0x12, 0x12
            };

        private byte[] DefaultIV => new byte[16]
        {
            1, 2, 3, 4, 5, 6, 7, 8,
            9, 10, 11, 12, 13, 14, 15, 16
        };

        private byte[] _currentKey;

        public Encrypter()
        {
            _currentKey = DefaultKey;
        }

        public void OverrideKey(byte[] key)
        {
            if (key.Length < 16) throw new ArgumentException($"Key is not valid. The key must have a length of 16 characters.");
            _currentKey = key;
        }

        public CryptoStream CreateEncryptionStream(FileStream fileStream)
        {
            using Aes aes = Aes.Create();
            CryptoStream cryptoStream = new CryptoStream(
                fileStream, aes.CreateEncryptor(_currentKey, DefaultIV), CryptoStreamMode.Write);
            return cryptoStream;
        }

        public CryptoStream CreateDecryptionStream(FileStream fileStream)
        {
            Aes aes = Aes.Create();
            CryptoStream cryptoStream = new CryptoStream(
                fileStream, aes.CreateEncryptor(_currentKey, DefaultIV), CryptoStreamMode.Read);
            return cryptoStream;
        }
    }
}
