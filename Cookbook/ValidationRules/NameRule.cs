using System;
using System.Globalization;
using System.Windows.Controls;

namespace Cookbook.ValidationRules;

public class NameRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if(string.IsNullOrWhiteSpace(value.ToString()))
            return new ValidationResult(false, $"Имя не может быть пустым");

        return ValidationResult.ValidResult;
    }
}