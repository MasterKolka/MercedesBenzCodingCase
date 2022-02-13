using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UrlShortener.Validation
{
    /// <summary>
    ///     Validator for the ShortName
    /// </summary>
    public class ShortNameAttribute : ValidationAttribute
    {
        private const string RegexPattern = "^[\\p{L}\\p{N}\\-_%.]+$";
        private static readonly Regex _shortNameRegex = new Regex(RegexPattern, RegexOptions.Compiled);

        /// <summary>
        ///     IsValid
        /// </summary>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var shortName = value as string;
            if (!string.IsNullOrEmpty(shortName) && !_shortNameRegex.IsMatch(shortName))
            {
                return new ValidationResult
                    ($"Parameter {validationContext.DisplayName} is invalid. Regex: {RegexPattern}");
            }
            return ValidationResult.Success!;
        }
    }
}
