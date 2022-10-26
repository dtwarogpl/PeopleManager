using System;
using System.Collections.Generic;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel.Helpers;

/// <summary>
/// https://learn.microsoft.com/en-us/troubleshoot/developer/dotnet/framework/general/argumentexception-select-row-wpf-datagrid
/// Can't use default Equality overrides in PersonDto. 
/// </summary>
internal class PersonEqualityComparer : IEqualityComparer<PersonDto>
{
    public bool Equals(PersonDto x, PersonDto y)
    {
        return x.HasSamePropertiesAs(y);
    }

    public int GetHashCode(PersonDto obj)
    {
        var hashCode = new HashCode();
        hashCode.Add(obj.ApartmentNumber);
        hashCode.Add(obj.DateOfBirth);
        hashCode.Add(obj.FirstName);
        hashCode.Add(obj.HouseNumber);
        hashCode.Add(obj.LastName);
        hashCode.Add(obj.PhoneNumber);
        hashCode.Add(obj.PostalCode);
        hashCode.Add(obj.StreetName);
        hashCode.Add(obj.Town);
        return hashCode.ToHashCode();
    }


}