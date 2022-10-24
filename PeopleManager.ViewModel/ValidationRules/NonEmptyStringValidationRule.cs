using System.Windows.Controls;

namespace PeopleManager.ViewModel.ValidationRules;

public  class NonEmptyStringValidationRule : ValidationRule 
{
    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    {
        string? input = value.ToString();
        if (string.IsNullOrEmpty(input)) return new ValidationResult(false, "Entry is required.");
        return new ValidationResult(true, null);
    }
   
}