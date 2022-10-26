using System;
using AutoMapper;
using PeopleManager.Domain.Models;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel.Automapper;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonDto>().ReverseMap();
    }
}