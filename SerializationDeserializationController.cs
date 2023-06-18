namespace Serialization
{
    public class SerializationDeserializationController
    {
        private readonly bool _runningCondition;
        private static readonly List<IMySerializable> _serializables = new();
        private static readonly MySerializer _mySerializer = new();
        private static float _delayS;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runningCondition">While runningCondition is set to true, autoserializing will be working</param>
        /// <param name="delayS">Delay of serialization triggering</param>
        public SerializationDeserializationController(ref bool runningCondition, float delayS)
        {
            _runningCondition = runningCondition;
            _delayS = delayS;
            StartAutoSaving();
        }
        /// <summary>
        /// Adds the IGameStats object to a controller and returns a deserialized object.
        /// </summary>
        public static T AddToController<T>(T gameStats) where T : Serializable<T>, new()
        {
            gameStats = MySerializer.Deserialize(gameStats, typeof(T));
            _serializables.Add(gameStats);
            return gameStats;
        }
        /// <summary>
        /// Serializes all objects in _serializables List
        /// </summary>
        private static void SerializeAll()
        {
            foreach (var gameStats in _serializables)
            {
                MySerializer.Serialize(gameStats, gameStats.GetType());
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
