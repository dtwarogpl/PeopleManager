using PeopleManager.Domain.Models;

namespace PeopleManager.Infrastructure;

public class DataStorageModel
{
    public List<Person> People { get; set; } = new();

    public static DataStorageModel Create(List<Person> people)
    {
        return new DataStorageModel()
        {
            People = people
        };
    }

    public static DataStorageModel Empty()
    {
        return new DataStorageModel
        {
         
        };
    }
}