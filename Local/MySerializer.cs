namespace SerializationLibrary.Local
{
    using System.Xml.Serialization;

    internal class MySerializer
    {
        /// <summary>
        /// Serializes an IMySerializable object
        /// </summary>
        /// <param name="obj"> IMySerializable object that will be serialized </param>
        public void Serialize<T>(string path, T obj, Type type) where T : IMySerializable
        {
            while (true)
            {
                try
                {
                    XmlSerializer serializer = new(type);
                    using FileStream fileStream = new(Path.Combine(path, $"{obj.FileName}.mvsave"), FileMode.Create);
                    serializer.Serialize(fileStream, obj);
                    return;
                }
                catch (Exception exc)
                {
                    if (exc.GetType() == typeof(InvalidOperationException))
                    {
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Takes an object and returns a new object that is deseralized
        /// </summary>
        /// <param name="obj"> IMySerializable object that is going to be deserialized </param>
        /// <returns>Deserialized object</returns>
        public T Deserialize<T>(string path, T obj, Type type) where T : IMySerializable
        {
            try
            {
                XmlSerializer serializer = new(type);
                using FileStream fileStream = new(Path.Combine(path, $"{obj.FileName}.mvsave"), FileMode.Open);
                T? result = (T?)serializer.Deserialize(fileStream);
                return result == null ? throw new Exception("Deserialization Failed") : result;
            }
            catch
            {
                Serialize(path, obj, obj.GetType());
                return obj;
            }
        }
    }
}