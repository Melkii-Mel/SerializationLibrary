﻿using SerializationLibrary;
using SerializationLibrary.Local;

namespace Serialization
{
    internal class SerializationDeserializationController
    {
        private readonly bool _runningCondition;
        private readonly float _delayS;
        private readonly string _folderPath;
        private static readonly List<IMySerializable> _serializables = new();
        private static readonly MySerializer _mySerializer = new();
        
        /// <param name="runningCondition">While runningCondition is set to true, autoserializing will be working</param>
        /// <param name="delayS">Delay of serialization triggering</param>
        /// <param name="folderPath">Path to a folder where serialized data will be contained</param>
        public SerializationDeserializationController(string folderPath, ref bool runningCondition, float delayS)
        {
            _runningCondition = runningCondition;
            _delayS = delayS;
            _folderPath = folderPath;
            StartAutoSaving();
        }
        /// <summary>
        /// Adds the IGameStats object to a controller and returns a deserialized object.
        /// </summary>
        public Serializable<T> AddToController<T>(Serializable<T> gameStats) where T : Serializable<T>, new()
        {
            gameStats = _mySerializer.Deserialize(_folderPath, gameStats, typeof(T));
            _serializables.Add(gameStats);
            return gameStats;
        }
        /// <summary>
        /// Serializes all objects in _serializables List
        /// </summary>
        internal void SerializeAll()
        {
            foreach (var gameStats in _serializables)
            {
                _mySerializer.Serialize(_folderPath, gameStats, gameStats.GetType());
            }
        }
        /// <summary>
        /// Initializes the Timer that calls SerializeAll() method
        /// </summary>
        private void StartAutoSaving()
        {
            _ = Timer();
        }
        /// <summary>
        /// Calls SerializeAll() method that serializes all objects in _serializables List
        /// </summary>
        private async Task Timer()
        {
            while (_runningCondition)
            {
                await Task.Delay((int)(_delayS * 1000));
                SerializeAll();
            }
        }
    }
}