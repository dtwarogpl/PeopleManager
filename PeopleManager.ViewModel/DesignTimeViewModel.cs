using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Bogus;
using PeopleManager.ViewModel;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.View;

public class DesignTimeViewModel : INotifyPropertyChanged
{
    public ObservableCollection<PersonDto> People { get; set; } = new();

    public DesignTimeViewModel()
    {
        LoadFakeDataPeople();
    }

    private void LoadFakeDataPeople()
    {
        Randomizer.Seed = new Random(8675309);

        var faker = new Faker<PersonDto>()
            .RuleFor(x => x.ApartmentNumber, y => y.Random.Number(1, 100).ToString())
            .RuleFor(x => x.FirstName, y => y.Person.FirstName)
            .RuleFor(x => x.LastName, y => y.Person.LastName)
            .RuleFor(x => x.HouseNumber, y => y.Address.BuildingNumber())
            .RuleFor(x => x.PhoneNumber, y => y.Phone.PhoneNumber())
            .RuleFor(x => x.PostalCode, y => y.Address.ZipCode())
            .RuleFor(x => x.StreetName, y => y.Address.StreetName())
            .RuleFor(x => x.Town, y => y.Address.City())
            .RuleFor(x => x.DateOfBirth, y => y.Date.PastDateOnly());
      
        for (int i = 0; i < 100; i++)
        {
            People.Add(faker);
            OnPropertyChanged(nameof(People));
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



}