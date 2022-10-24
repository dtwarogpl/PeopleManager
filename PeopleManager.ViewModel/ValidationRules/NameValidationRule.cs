using Person = PeopleManager.Domain.Models.Person;

namespace PeopleManager.ViewModel.ValidationRules;

public class NameValidationRule : NonEmptyStringWithSpecificLengthValidationRule<Person>
{
    protected override string PropertyName => nameof(Person.FirstName);
}