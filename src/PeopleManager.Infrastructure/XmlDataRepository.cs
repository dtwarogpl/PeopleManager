using PeopleManager.Domain.Models;
using PeopleManager.Domain.Services;

namespace PeopleManager.Infrastructure;

public class XmlDataRepository  : IPeopleRepository
{
  

    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        await Task.Delay(1000);

      var data= new List<Person>()
        {
            new Person()
            {
                DateOfBirth = new DateOnly(1992, 11, 5),
                FirstName = "Dominik",
                LastName = "Twaróg",
                StreetName = "Piłsudskiego",
                Town = "Głogów Małopolski",
                ApartmentNumber = "36",
                HouseNumber = "26A",
                PhoneNumber = "518906575",
                PostalCode = "36-060"
            },
            new Person()
            {
                DateOfBirth = new DateOnly(1992, 11, 5),
                FirstName = "Natalia",
                LastName = "Twaróg",
                StreetName = "Piłsudskiego",
                Town = "Głogów Małopolski",

                HouseNumber = "26A",
                PhoneNumber = "518906575",
                PostalCode = "36-060"
            },
            new Person()
            {
                DateOfBirth = new DateOnly(1992, 11, 5),
                FirstName = "Ignacy",
                LastName = "Twaróg",
                StreetName = "Piłsudskiego",
                Town = "Głogów Małopolski",
                ApartmentNumber = "36",
                HouseNumber = "26A",
                PhoneNumber = "518906575",
                PostalCode = "36-060"
            }
        };

      return data;
    }
}