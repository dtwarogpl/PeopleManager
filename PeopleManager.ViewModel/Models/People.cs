using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PeopleManager.Domain.Models;
using PeopleManager.Domain.Services;
using PeopleManager.ViewModel.Helpers;

namespace PeopleManager.ViewModel.Models;

public class People : ObservableCollection<PersonDto>
{
    private readonly IEqualityComparer<PersonDto> _comparer;
    private List<PersonDto> _snapShot = new();

    public People(List<PersonDto> source) : base(source)
    {
        _snapShot = new List<PersonDto>(source.Select(x => x.Duplicate()));
        _comparer = new PersonEqualityComparer();
    }

    public People()
    {
        _comparer = new PersonEqualityComparer();
    }

    public bool HaveBeenChanged()
    {
        return !CurrentStateIsEqualToSnapShoot();
    }

    public bool CanBeSaved()
    {
        var allHaveGoodState = this.All(y => !y.IsCorrupted);
        return allHaveGoodState && !CurrentStateIsEqualToSnapShoot();
    }

    public async Task SaveAsync(IMapper mapper, IPeopleRepository repository)
    {
        var peopleSource = mapper.Map<List<Person>>(this.ToList());
        await repository.SavePeopleAsync(peopleSource);
        SaveSnapShot();
    }

    private bool CurrentStateIsEqualToSnapShoot()
    {
        return this.SequenceEqual(_snapShot, _comparer);
    }

    private void SaveSnapShot()
    {
        _snapShot = new List<PersonDto>(this.Select(x => x.Duplicate()));
    }
}