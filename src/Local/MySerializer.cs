namespace SerializationLibrary.Local
{
    using Serialization.src.Local;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Xml.Serialization;

    internal class MySerializer
    {
        private readonly Encrypter encrypter = new Encrypter();

        /// <summary>
        /// Serializes an IMySerializable object
        /// </summary>
        /// <param name="obj"> IMySerializable object that will be serialized </param>
        public void Serialize<T>(string path, T obj, Type type, bool encrypt) where T : ISerializable
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using FileStream fileStream = new FileStream(Path.Combine(path, $"{obj.FileName}.mvsave"), FileMode.Create);

            if (encrypt)
            {
                using CryptoStream encryptionStream = encrypter.CreateEncryptionStream(fileStream);
                serializer.Serialize(encryptionStream, obj);
            }
            else
            {
                serializer.Serialize(fileStream, obj);
            }
            return;
        }
        /// <summary>
        /// Takes an object and returns a new object that is deseralized
        /// </summary>
        /// <param name="obj"> IMySerializable object that is going to be deserialized </param>
        /// <returns>Deserialized object</returns>
        public T Deserialize<T>(string path, T obj, Type type, bool decrypt) where T : class, ISerializable
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                using FileStream fileStream = new FileStream(Path.Combine(path, $"{obj.FileName}.mvsave"), FileMode.Open);
                T? result;
                if (decrypt)
                {
                    using CryptoStream decryptionStream = encrypter.CreateDecryptionStream(fileStream);
                    result = (T?)serializer.Deserialize(decryptionStream);
                }
                else
                {
                    result = (T?)serializer.Deserialize(fileStream);
                }
                if (result == null)
                {
                    throw new Exception("Deserialization Failed");
                }
                result.Decrypt = decrypt;
                return result;
            }
            catch
            {
                Serialize(path, obj, obj.GetType(), decrypt);
                return obj;
            }
        }
    }
}