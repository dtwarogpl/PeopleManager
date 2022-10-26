using System.Xml.Serialization;

namespace PeopleManager.Infrastructure.Services.Serialization;

public class XmlDataSerializer<T> : IDataSerializer<T> where T : class, new()
{
    public string Serialize(T data)
    {
        using var writer = new StringWriter();
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, data);
        return writer.ToString();
    }

    public T Deserialize(string data)
    {
        using var stringReader = new StringReader(data);
        var serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(stringReader) as T ?? new T();
    }
}