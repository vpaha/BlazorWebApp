using Reparo.Shared.Models.Resource;
using System.ComponentModel.DataAnnotations;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly object[] _ifValues;

    public RequiredIfAttribute(string propertyName, object[] ifValues)
    {
        _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        _ifValues = ifValues;
    }

    public override string FormatErrorMessage(string name)
    {
        var errorMessage = ModelResource.MandatoryFieldMessage;
        return ErrorMessage ?? errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(validationContext);
        var property = validationContext.ObjectType.GetProperty(_propertyName);

        if (property == null)
        {
            throw new NotSupportedException($"Can't find {_propertyName} on searched type: {validationContext.ObjectType.Name}");
        }

        var requiredIfTypeActualValue = property.GetValue(validationContext.ObjectInstance);

        if (requiredIfTypeActualValue == null || _ifValues == null || _ifValues.Length == 0)
        {
            return ValidationResult.Success;
        }

        if (requiredIfTypeActualValue == null || _ifValues.Contains(requiredIfTypeActualValue))
        {
            return (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
                : ValidationResult.Success;
        }

        return ValidationResult.Success;
    }
}