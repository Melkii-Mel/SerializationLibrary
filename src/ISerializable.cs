namespace SerializationLibrary.Local
{
    public interface ISerializable
    {
        public string FileName { get; }

        internal bool Decrypt { get; }
    }
}