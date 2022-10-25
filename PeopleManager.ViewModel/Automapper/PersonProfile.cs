using System;
using AutoMapper;
using PeopleManager.Domain.Models;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel.Automapper;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonDto, Person>().ForMember(x=>x.DateOfBirth,o=>o.MapFrom(src=>DateOnly.FromDateTime(src.DateOfBirth)));
        CreateMap<Person, PersonDto>().ForMember(x=>x.DateOfBirth,o=>o.MapFrom(src=>new DateTime(src.DateOfBirth.Year, src.DateOfBirth.Month, src.DateOfBirth.Day)));
    }
}