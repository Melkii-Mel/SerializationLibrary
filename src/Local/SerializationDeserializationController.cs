using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SerializationLibrary.Local
{
    internal class SerializationDeserializationController
    {
        private readonly bool _runningCondition;
        private readonly Func<bool>? _isRunning;
        private readonly float _delayS;
        private readonly string _folderPath;
        private readonly List<IMySerializable> _serializables = new List<IMySerializable>();
        private readonly MySerializer _mySerializer = new MySerializer();

        /// <param name="runningCondition">While runningCondition is set to true, autoserializing will be working</param>
        /// <param name="delayS">Delay of serialization triggering</param>
        /// <param name="folderPath">Path to a folder where serialized data will be contained</param>
        public SerializationDeserializationController(string folderPath, in bool runningCondition, float delayS)
        {
            _runningCondition = runningCondition;
            _delayS = delayS;
            _folderPath = folderPath;
            StartAutoSaving();
        }
        public SerializationDeserializationController(string folderPath, Func<bool> isRunning, float delayS)
        {
            _runningCondition = false;
            _folderPath = folderPath;
            _isRunning = isRunning;
            _delayS = delayS;
            StartAutoSaving();
        }
        /// <summary>
        /// Adds the IGameStats object to a controller and returns a deserialized object.
        /// </summary>
        public Serializable<T> AddToController<T>(Serializable<T> gameStats, bool deserialize = true) where T : Serializable<T>, new()
        {
            if (deserialize)
            {
                gameStats = _mySerializer.Deserialize(_folderPath, gameStats, typeof(T));
            }
            else
            {
                _mySerializer.Serialize(_folderPath, gameStats, typeof(T));
            }
            _serializables.Add(gameStats);
            return gameStats;
        }
        /// <summary>
        /// Serializes all objects in _serializables List
        /// </summary>
        internal void SerializeAll()
        {
            foreach (var serializable in _serializables)
            {
                _mySerializer.Serialize(_folderPath, serializable, serializable.GetType());
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
            while (_runningCondition || _isRunning != null && _isRunning())
            {
                await Task.Delay((int)(_delayS * 1000));
                SerializeAll();
            }
        }
    }
}
