using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace PeopleManager.Domain.Models;

public record Person
{
    [Required]
    [MinLength(3)]
    public string FirstName { get; init; }
    [Required]
    [MinLength(3)]
    public string LastName { get; init; }
    [Required]
    [MinLength(3)]
    public string StreetName { get; init; }
    [Required]
    public string HouseNumber { get; init; }

    public string? ApartmentNumber { get; init; }
    [Required]
    [MinLength(3)]
    public string PostalCode { get; init; }
    [Required]
    [MinLength(3)]
    public string Town { get; init; }
    [Required]
    [MinLength(3)]
    public string PhoneNumber { get; init; }
    [Required]
    public DateOnly DateOfBirth { get; init; }



   
}