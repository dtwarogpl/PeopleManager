﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Linq;

namespace PeopleManager.ViewModel.Models;

public class PersonDto : INotifyPropertyChanged
{
    private string _firstName;
    private string _lastName;
    private string _streetName;
    private string _houseNumber;
    private string? _apartmentNumber;
    private string _postalCode;
    private string _town;
    private string _phoneNumber;
    private DateOnly _dateOfBirth;

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

    public DateOnly DateOfBirth
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
        int age = now.Year - DateOfBirth.Year;
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
        return true;
    }
}