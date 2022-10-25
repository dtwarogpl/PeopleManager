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
    private DateTime _dateOfBirth;
    private string _firstName;
    private string _houseNumber;
    private string _lastName;
    private string _phoneNumber;
    private string _postalCode;
    private string _streetName;
    private string _town;

    private PersonDtoSnapshot? SnapShot { get; set; }

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

    public void CreateSnapShot()
    {
        SnapShot = new PersonDtoSnapshot(_firstName, _lastName, _streetName, _houseNumber, _apartmentNumber,
            _postalCode, _town,
            _phoneNumber, _dateOfBirth);
    }

    public bool HasBeenModified()
    {
        return SnapShot != null && !SnapShot.Equals(this);
    }

    private int? CalculateAge()
    {
        var now = DateTime.Today;
        var age = now.Year - DateOfBirth.Year;
        if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
            age--;
        return age;
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
            DateOfBirth = new DateOnly(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day)
        };

        var validationResult = domainModel.Validate();
        return validationResult;
    }
}