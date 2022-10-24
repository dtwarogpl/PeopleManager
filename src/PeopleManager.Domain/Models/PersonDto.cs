using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace PeopleManager.Domain.Models;

public record Person
{
    [MaxLength(5)]
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string StreetName { get; init; }
    public string HouseNumber { get; init; }
    public string? ApartmentNumber { get; init; }
    public string PostalCode { get; init; }
    public string Town { get; init; }
    public string PhoneNumber { get; init; }
    public DateOnly DateOfBirth { get; init; }
   
}