using Serialization.src.Local;
using System;
using System.Xml.Serialization;

namespace Serialization.src
{
    [Serializable]
    public abstract class Serializable<T> : IMySerializable where T : Serializable<T>, new()
    {
        private int index;
        internal static T Create(int index, SerializationDeserializationController controller)
        {
            Serializable<T> result = new T
            {
                index = index
            };
            result = controller.AddToController(result);
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
