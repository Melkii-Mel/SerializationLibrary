using Serialization;

namespace SerializationLibrary
{
    public abstract class SerializablesHolder
    {
        private readonly int index;
        private readonly SerializationDeserializationController serializationDeserializationController;
        public SerializablesHolder(int serializablesIndex, in bool runningCondition, float delayS)
        {
            serializationDeserializationController = new("", in runningCondition, delayS);
            index = serializablesIndex;
        }
        public T CreateSerializable<T>() where T : Serializable<T>, new()
        {
            return Serializable<T>.Create(index, serializationDeserializationController);
        }
        public void SerializeAll()
        {
            serializationDeserializationController.SerializeAll();
        }
    }
}
