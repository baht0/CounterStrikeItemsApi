using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CounterStrikeItemsApi.Application.Helpers
{
    public class SlugListAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Список необязательный

            if (value is List<string> list)
            {
                var regex = new Regex(@"^[a-z0-9]+(-[a-z0-9]+)*$", RegexOptions.Compiled);

                foreach (var item in list)
                {
                    if (item != null && !regex.IsMatch(item))
                    {
                        return new ValidationResult(
                            $"Value '{item}' is not a valid slug. " +
                            "Allowed format: lowercase letters, numbers, hyphens (e.g. 'ak-47-redline')."
                        );
                    }
                }
            }
            else
            {
                return new ValidationResult("The field must be a list of strings.");
            }

            return ValidationResult.Success;
        }
    }
}
