namespace Serialization
{
    [Serializable]
    public abstract class Serializable<T> : IMySerializable where T : Serializable<T>, new()
    {
        private static Serializable<T>? _instance;
        public static T Instance
        {
            get
            {
                _instance ??= new T();
                _instance = SerializationDeserializationController.AddToController((T)_instance);
                return (T)_instance;
            }
            set
            {
                _instance = value;
            }
        }

        public string FileName
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Stupid developer exception");
                }
                return _instance.GetType().ToString();
            }
        }
    }
}
