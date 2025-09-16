using System.Text;
using System.Text.RegularExpressions;

namespace CounterStrikeItemsApi.Application.Helpers
{
    public static class StringToSlug
    {
        public static string Generate(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string slug = input.ToLowerInvariant();
            // Удаляем диакритические знаки (например, é → e)
            slug = RemoveDiacritics(slug);
            // Заменяем всё, кроме букв, цифр и дефисов, на дефисы
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "-");
            // Заменяем пробелы на дефисы
            slug = Regex.Replace(slug, @"[\s-]+", "-");
            slug = slug.Trim('-');

            return slug;
        }
        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
