using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _otherPropertyName;

    public DateGreaterThanAttribute(string otherPropertyName)
    {
        _otherPropertyName = otherPropertyName;
        ErrorMessage = "{0} deve ser maior que {1}.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
        if (otherProperty == null)
            return new ValidationResult($"Propriedade {_otherPropertyName} não encontrada.");

        var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

        if (value == null || otherValue == null)
            return ValidationResult.Success;

        if (value is DateTime currentDate && otherValue is DateTime otherDate)
        {
            if (currentDate > otherDate)
                return ValidationResult.Success;
            else
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName));
        }
        return new ValidationResult("Valores inválidos para comparação de datas.");
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, _otherPropertyName);
    }
}
