namespace Serialization
{
    using System.Xml.Serialization;
    internal class MySerializer
    {
        /// <summary>
        /// Serializes an IMySerializable object
        /// </summary>
        /// <param name="obj"> IMySerializable object that will be serialized </param>
        public static void Serialize<T>(T obj, Type type) where T : IMySerializable
        {
            while (true)
            {
                try
                {
                    XmlSerializer serializer = new(type);
                    using FileStream fileStream = new($"{obj.FileName}.mvsave", FileMode.Create);
                    serializer.Serialize(fileStream, obj);
                    break;
                }
                catch { }
            }
        }
        /// <summary>
        /// Takes an object and returns a new object that is deseralized
        /// </summary>
        /// <param name="obj"> IMySerializable object that is going to be deserialized </param>
        /// <returns>Deserialized object</returns>
        public static T Deserialize<T>(T obj, Type type) where T : IMySerializable
        {
            try
            {
                XmlSerializer serializer = new(type);
                using FileStream fileStream = new($"{obj.FileName}.mvsave", FileMode.Open);
                T? result = (T?)serializer.Deserialize(fileStream);
                return result == null ? throw new Exception("Deserialization Failed") : result;
            }
            catch
            {
                Serialize(obj, obj.GetType());
                return obj;
            }
        }
    }
}