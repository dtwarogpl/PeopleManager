using System.Linq;
using System;

namespace PeopleManager.ViewModel.Helpers;

public static class ValidatorExtractor
{
    public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
    {
        var attrType = typeof(T);
        var property = instance.GetType().GetProperty(propertyName);
        return (T)property.GetCustomAttributes(attrType, false).First();
    }
}