using PeopleManager.Domain.Models;

namespace PeopleManager.Domain.Services;

public interface IPeopleRepository
{
    public Task<IEnumerable<Person>> GetPeopleAsync();
    public Task SavePeopleAsync(List<Person>people);
}