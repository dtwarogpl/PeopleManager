using AutoMapper;
using PeopleManager.Domain.Models;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel.Automapper;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonDto, Person>().ReverseMap();
    }
}