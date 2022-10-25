using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using PeopleManager.Domain.Models;
using PeopleManager.ViewModel.Helpers;

namespace PeopleManager.ViewModel.Models;

public class PersonDto : INotifyPropertyChanged
{
    private record SnapShotDto(string FirstName, string LastName,string StreetName, string HouseNumber, string ApartmentNumber, string PostalCode, string Town, string PhoneNumber, DateTime DateOfBirth)
    {
       public bool Equals(PersonDto dto)
        {
            return string.Equals(FirstName, dto.FirstName) &&
                   string.Equals(LastName, dto.LastName) &&
                   string.Equals(StreetName, dto.StreetName) &&
                   string.Equals(HouseNumber, dto.HouseNumber) &&
                   string.Equals(ApartmentNumber, dto.ApartmentNumber) &&
                   string.Equals(PostalCode, dto.PostalCode) &&
                   string.Equals(PostalCode, dto.PostalCode) &&
                   string.Equals(Town, dto.Town) &&
                   string.Equals(PhoneNumber, dto.PhoneNumber) &&
                   DateOfBirth.Equals(dto.DateOfBirth);
        }
    }

    private string _firstName;
    private string _lastName;
    private string _streetName;
    private string _houseNumber;
    private string? _apartmentNumber;
    private string _postalCode;
    private string _town;
    private string _phoneNumber;
    private DateTime _dateOfBirth;


    private SnapShotDto? SnapShot { get; set; }

    public void CreateSnapShot()
    {
        SnapShot = new(_firstName, _lastName, _streetName, _houseNumber, _apartmentNumber, _postalCode, _town,
            _phoneNumber, _dateOfBirth);
    }

    public bool HasBeenModified() => SnapShot != null && !SnapShot.Equals(this);


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
            if (value.Equals(_dateOfBirth)) return;
            _dateOfBirth = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Age));
        }
    }

    public int? Age => CalculateAge();
    private int? CalculateAge()
    {
        var now = DateTime.Today;
        var age = now.Year - DateOfBirth.Year;
        if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
            age--;
        return age;
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        OnPropertyChanged(nameof(IsCorrupted));
        OnPropertyChanged(nameof(ModelValidationErrors));
        return true;
    }
    public bool IsCorrupted => GetValidationResults().Any();
    public IEnumerable<string> ValidationResults => GetValidationResults()
        .Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).Select(x => x.ErrorMessage!);
    private IEnumerable<ValidationResult> GetValidationResults()
    {
        var domainModel = new Person()
        {
            FirstName = FirstName,
            LastName = LastName,
            StreetName = StreetName,
            HouseNumber = HouseNumber,
            ApartmentNumber = ApartmentNumber,
            PostalCode = PostalCode,
            Town = Town,
            PhoneNumber = PhoneNumber,
            DateOfBirth = new DateOnly(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day),
        };

        var validationResult = domainModel.Validate();
        return validationResult;
    }
    public string? ModelValidationErrors => string.Join(Environment.NewLine, ValidationResults);
}