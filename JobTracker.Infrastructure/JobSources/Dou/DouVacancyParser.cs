using AngleSharp.Dom;
using JobTracker.Application.Common.Abstractions;
using System.Text.RegularExpressions;

namespace JobTracker.Infrastructure.Dou
{
    public sealed class DouVacancyParser
    {
        private const string BaseUrl = "https://jobs.dou.ua";

        private static readonly Dictionary<string, int> UkrainianMonths = new()
        {
            ["січня"] = 1,
            ["лютого"] = 2,
            ["березня"] = 3,
            ["квітня"] = 4,
            ["травня"] = 5,
            ["червня"] = 6,
            ["липня"] = 7,
            ["серпня"] = 8,
            ["вересня"] = 9,
            ["жовтня"] = 10,
            ["листопада"] = 11,
            ["грудня"] = 12
        };
        public JobSourceVacancy? ParseVacancy(IElement element)
        {
            var titleElement = element.QuerySelector("a.vt");
            var title = titleElement?.TextContent.Trim();

            if (string.IsNullOrEmpty(title))
            {
                return null;
            };

            var href = titleElement?.GetAttribute("href");
            if (string.IsNullOrWhiteSpace(href))
            {
                return null;
            }

            var url = MakeAbsoluteUrl(href);
            var externalId = ExtractExternalId(url);

            var companyElement = element.QuerySelector("a.company");
            var company = companyElement?.TextContent.Trim();

            // Извлекаем локацию
            var locationElement = element.QuerySelector("span.cities");
            var location = locationElement?.TextContent.Trim();

            // Определяем, удалённая ли вакансия (с проверкой на null)
            var isRemote = location?.Contains("remote", StringComparison.OrdinalIgnoreCase) == true ||
                           location?.Contains("віддалено", StringComparison.OrdinalIgnoreCase) == true;

            // Извлекаем зарплату
            var salaryText = FindSalaryText(element);
            var (salaryMin, salaryMax, currency) = ParseSalary(salaryText);

            // Извлекаем описание
            var descriptionElement = element.QuerySelector("div.sh-info");
            var description = descriptionElement?.TextContent.Trim();

            //date
            var dateElement = element.QuerySelector("div.date");
            var publishedAt = ParsePublishedDate(dateElement?.TextContent.Trim());

            return new JobSourceVacancy(
                ExternalId: externalId,
                Title: title,
                Company: company,
                Location: location,
                IsRemote: isRemote,
                SalaryMin: salaryMin,
                SalaryMax: salaryMax,
                Currency: currency,
                Url: url,
                Description: description,
                PublishedAt: publishedAt);
        }

        private static string MakeAbsoluteUrl(string href)
        {
            if(Uri.TryCreate(href,UriKind.Absolute,out var absoluteUri))
            {
                return absoluteUri.ToString();
            }

            return Uri.TryCreate(new Uri(BaseUrl), href, out var combined)
                ? combined.ToString()
                : href;
        }

        private static string ExtractExternalId(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return url;
            }

            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return segments.Length > 0 ? segments[^1] : url;
        }

        private static string? FindSalaryText(IElement element)
        {
            foreach (var candidate in element.QuerySelectorAll("span, div, strong, b"))
            {
                if (candidate.Children.Length > 0)
                {
                    continue;
                }

                var text = candidate.TextContent.Trim();

                if (text.Length > 0 && text.Length < 40 &&
                    Regex.IsMatch(text, @"(\$|€|₴)\s?\d"))
                {
                    return text;
                }
            }

            return null;
        }
        private static (int? Min, int? Max, string? Currency) ParseSalary(string? salaryText)
        {
            if (string.IsNullOrWhiteSpace(salaryText))
            {
                return (null, null, null);
            }

            var currency = salaryText.Contains('$') ? "USD"
                : salaryText.Contains('€') ? "EUR"
                : salaryText.Contains('₴') ? "UAH"
                : null;

            var numbers = Regex.Matches(salaryText, @"\d+")
                .Select(m => int.Parse(m.Value))
                .ToList();

            if (numbers.Count == 0)
            {
                return (null, null, currency);
            }

            var isFrom = Regex.IsMatch(salaryText, @"\bвід\b", RegexOptions.IgnoreCase);
            var isUpTo = Regex.IsMatch(salaryText, @"\bдо\b", RegexOptions.IgnoreCase);

            if (numbers.Count == 1)
            {
                // "до $5500" → только Max
                // "від $1800" → только Min
                return isUpTo && !isFrom
                    ? (null, numbers[0], currency)
                    : (numbers[0], null, currency);
            }

            // "$1200–1650" → диапазон
            return (numbers[0], numbers[^1], currency);
        }

        private static DateTimeOffset? ParsePublishedDate(string? dateText)
        {
            if (string.IsNullOrWhiteSpace(dateText))
            {
                return null;
            }

            var parts = dateText.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2 || !int.TryParse(parts[0], out var day))
            {
                return null;
            }

            if (!UkrainianMonths.TryGetValue(parts[1].ToLowerInvariant(), out var month))
            {
                return null;
            }
            var now = DateTimeOffset.UtcNow;
            var year = now.Year;

            if (day > DateTime.DaysInMonth(year, month))
            {
                return null;
            }

            var date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

            // "31 грудня" когда на дворе январь это прошлый год
            if (date > now)
            {
                date = date.AddYears(-1);
            }

            return date;
        }

    }
}
