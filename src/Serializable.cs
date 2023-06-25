using SerializationLibrary.Local;
using System;
using System.Reflection;

namespace SerializationLibrary
{
    [Serializable]
    public abstract class Serializable<T> : ISerializable where T : Serializable<T>, new()
    {
        private int index;
        internal static T Create(int index, SerializationDeserializationController controller)
        {
            Serializable<T> result = new T
            {
                index = index,
            };
            result = controller.AddToController(result);
            return (T)result;
        }
        internal static T CreateEmpty(int index, SerializationDeserializationController controller)
        {
            Serializable<T> result = new T
            {
                index = index,
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
