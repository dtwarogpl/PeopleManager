namespace PeopleManager.Infrastructure.Services.Serialization;

public interface IDataSerializer<T>
{
    public string Serialize(T data);
    public T Deserialize(string data);
}