using System;

namespace PeopleManager.ViewModel.Models;

internal record PersonDtoSnapshot(string FirstName, string LastName, string StreetName, string HouseNumber, string? ApartmentNumber, string PostalCode, string Town, string PhoneNumber, DateTime DateOfBirth)
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