using SerializationLibrary.Local;
using System;
using System.Reflection;
using System.Threading;

namespace SerializationLibrary
{
    [Serializable]
    public abstract class Serializable<T> : ISerializable where T : Serializable<T>, new()
    {
        private int index;
        private bool _decrypt;
        bool ISerializable.Decrypt => _decrypt;
        internal static T Create(int index, SerializationDeserializationController controller, bool decrypt = false)
        {
            Serializable<T> result = new T
            {
                index = index,
                _decrypt = decrypt,
            };
            result = controller.AddToController(result);
            return (T)result;
        }
        internal static T CreateEmpty(int index, SerializationDeserializationController controller, bool decrypt = false)
        {
            Serializable<T> result = new T
            {
                index = index,
                _decrypt = decrypt,
            };
            result = controller.AddToController(result, deserialize: false);
            return (T)result;
        }
        public string FileName
        {
            get
            {
                return $"{GetType()}{index}";
            }
        }
    }
}
