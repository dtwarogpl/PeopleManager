using System;
using System.Windows.Input;
using System.Xml.Linq;

namespace PeopleManager.ViewModel.Models;

public class PersonDto 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StreetName { get; set; }
    public string HouseNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; }
    public string Town { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public int? Age => CalculateAge();
    private int? CalculateAge()
    {
       
        var now = DateTime.Today;
        int age = now.Year - DateOfBirth.Year;
        if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
            age--;
        return age;
    }
}