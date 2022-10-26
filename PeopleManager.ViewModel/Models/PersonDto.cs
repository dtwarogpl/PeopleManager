using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PeopleManager.Domain.Models;
using PeopleManager.ViewModel.Helpers;

namespace PeopleManager.ViewModel.Models;

public class PersonDto : ObservableDto
{
    private string? _apartmentNumber;
    private DateTime _dateOfBirth = DateTime.Today;
    private string? _firstName;
    private string? _houseNumber;
    private string? _lastName;
    private string? _phoneNumber;
    private string? _postalCode;
    private string? _streetName;
    private string? _town;
   

    public string FirstName
    {
        get => _firstName;
        set => SetField(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetField(ref _lastName, value);
    }

    public string StreetName
    {
        get => _streetName;
        set => SetField(ref _streetName, value);
    }

    public string HouseNumber
    {
        get => _houseNumber;
        set => SetField(ref _houseNumber, value);
    }

    public string? ApartmentNumber
    {
        get => _apartmentNumber;
        set => SetField(ref _apartmentNumber, value);
    }

    public string PostalCode
    {
        get => _postalCode;
        set => SetField(ref _postalCode, value);
    }

    public string Town
    {
        get => _town;
        set => SetField(ref _town, value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetField(ref _phoneNumber, value);
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            SetField(ref _dateOfBirth, value);
            OnPropertyChanged(nameof(Age));
        }
    }

    public int? Age => CalculateAge();

    protected override List<string> AlwaysNotifyFieldNames => new()
    {
        nameof(IsCorrupted), nameof(ModelValidationErrors)
    };

    public bool IsCorrupted => GetValidationResults().Any();

    public IEnumerable<string> ValidationResults => GetValidationResults()
        .Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).Select(x => x.ErrorMessage!);

    public string? ModelValidationErrors => string.Join(Environment.NewLine, ValidationResults);

    private int? CalculateAge()
    {
        var now = DateTime.Today;
        var age = now.Year - DateOfBirth.Year;
        if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
            age--;
        return age;
    }
    public bool HasSamePropertiesAs(PersonDto other)
    {
        return _apartmentNumber == other._apartmentNumber && _dateOfBirth.Equals(other._dateOfBirth) && _firstName == other._firstName && _houseNumber == other._houseNumber && _lastName == other._lastName && _phoneNumber == other._phoneNumber && _postalCode == other._postalCode && _streetName == other._streetName && _town == other._town;
    }
    public  PersonDto Duplicate()
    {
       return new PersonDto()
        {
            FirstName = FirstName,
            LastName = LastName,
            StreetName = StreetName,
            HouseNumber = HouseNumber,
            ApartmentNumber = ApartmentNumber,
            PostalCode = PostalCode,
            Town = Town,
            PhoneNumber = PhoneNumber,
            DateOfBirth = DateOfBirth
        };
    }
    private IEnumerable<ValidationResult> GetValidationResults()
    {
        var domainModel = new Person
        {
            FirstName = FirstName,
            LastName = LastName,
            StreetName = StreetName,
            HouseNumber = HouseNumber,
            ApartmentNumber = ApartmentNumber,
            PostalCode = PostalCode,
            Town = Town,
            PhoneNumber = PhoneNumber,
            DateOfBirth = DateOfBirth
        };

        return domainModel.Validate();
    }
}