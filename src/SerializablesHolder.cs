﻿using Serialization.src.Local;
using System;

namespace Serialization.src
{
    public abstract class SerializablesHolder
    {
        private readonly int index;
        private readonly SerializationDeserializationController serializationDeserializationController;
        public SerializablesHolder(int serializablesIndex, in bool runningCondition, float delayS, string path, params Action[] serializationTriggers)
        {
            serializationDeserializationController = new SerializationDeserializationController(path, in runningCondition, delayS);
            index = serializablesIndex;
            AddSerializationsToActions(serializationTriggers);
        }
        public SerializablesHolder(int serializablesIndex, Func<bool> isRunning, float delayS, string path, params Action[] serializationTriggers)
        {
            serializationDeserializationController = new SerializationDeserializationController(path, isRunning, delayS);
            index = serializablesIndex;
            AddSerializationsToActions(serializationTriggers);
        }
        public T CreateSerializable<T>() where T : Serializable<T>, new()
        {
            return Serializable<T>.Create(index, serializationDeserializationController);
        }
        public void SerializeAll()
        {
            serializationDeserializationController.SerializeAll();
        }
        public void AddSerializationTrigger(Action action)
        {
            action += SerializeAll;
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