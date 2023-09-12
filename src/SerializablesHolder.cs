using SerializationLibrary.Local;
using System;

namespace SerializationLibrary
{
    public abstract class SerializablesHolder
    {
        protected readonly int index;
        private protected readonly SerializationDeserializationController serializationDeserializationController;
        public bool Encrypt { get; private set; }

        public SerializablesHolder(int serializablesIndex, in bool runningCondition, float delayS, string path, bool encrypt = false, params Action[] serializationTriggers)
        {
            Encrypt = encrypt;
            serializationDeserializationController = new SerializationDeserializationController(path, in runningCondition, delayS);
            index = serializablesIndex;
            AddSerializationsToActions(serializationTriggers);
        }
        
        public SerializablesHolder(int serializablesIndex, Func<bool> isRunning, float delayS, string path, bool encrypt = false, params Action[] serializationTriggers)
        {
            Encrypt = encrypt;
            serializationDeserializationController = new SerializationDeserializationController(path, isRunning, delayS);
            index = serializablesIndex;
            AddSerializationsToActions(serializationTriggers);
        }
        
        public T CreateSerializable<T>() where T : Serializable<T>, new()
        {
            return Serializable<T>.Create(index, serializationDeserializationController, Encrypt);
        }
        
        public T ResetSerializable<T>() where T : Serializable<T>, new()
        {
            return Serializable<T>.CreateEmpty(index, serializationDeserializationController, Encrypt);
        }
        
        public void SerializeAll()
        {
            serializationDeserializationController.SerializeAll();
        }
        
        public void AddSerializationTrigger(Action action)
        {
            action += SerializeAll;
        }
        
        /// <summary>
        /// enables or disables encryption for data safety. by default encryption is off
        /// </summary>
        public void SetEncryptionActive(bool active)
        {
            throw new NotImplementedException();
        }
        
        private void AddSerializationsToActions(Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                Action action = actions[i];
                action += SerializeAll;
            }
        }
    }
}
