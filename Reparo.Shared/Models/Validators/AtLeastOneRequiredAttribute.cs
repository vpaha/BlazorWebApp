using System.ComponentModel.DataAnnotations;

public class AtLeastOneRequiredAttribute : ValidationAttribute
{
    public readonly string[] _propertyNames;
    public AtLeastOneRequiredAttribute(params string[] propertyNames)
    {
        _propertyNames = propertyNames;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        foreach (var propertyName in _propertyNames)
        {
            var property = validationContext.ObjectType.GetProperty(propertyName);

            if (property == null)
                continue;

            var propertyValue = property.GetValue(validationContext.ObjectInstance) as string;
            if (!string.IsNullOrWhiteSpace(propertyValue))
                return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "Enter claim id or search for member or provider.");
    }
}