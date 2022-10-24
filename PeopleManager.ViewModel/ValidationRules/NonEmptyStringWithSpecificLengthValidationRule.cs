using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using PeopleManager.ViewModel.Helpers;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace PeopleManager.ViewModel.ValidationRules;

public abstract class NonEmptyStringWithSpecificLengthValidationRule<T> : ValidationRule where T : class, new()
{
    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    {
        var person = new T();
        var maxLength = person.GetAttributeFrom<MaxLengthAttribute>(PropertyName).Length;

        string? input = value.ToString();
        if (string.IsNullOrEmpty(input)) return new ValidationResult(false, "Entry is required.");
        if (input.Length >= maxLength) return new ValidationResult(false, "Response is invalid.");
        return new ValidationResult(true, null);
    }

    protected abstract string PropertyName { get; }
}