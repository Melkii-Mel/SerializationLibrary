using Serialization;

namespace SerializationLibrary
{
    public abstract class SerializablesHolder
    {
        private int index;
        private SerializationDeserializationController serializationDeserializationController;
        public SerializablesHolder(int serializablesIndex, ref bool runningCondition, float delayS)
        {
            serializationDeserializationController = new("", ref runningCondition, delayS);
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
