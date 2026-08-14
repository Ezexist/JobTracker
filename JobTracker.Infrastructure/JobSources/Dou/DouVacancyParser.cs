using AngleSharp.Dom;
using JobTracker.Application.Common.Abstractions;

namespace JobTracker.Infrastructure.Dou
{
    public sealed class DouVacancyParser
    {
        public JobSourceVacancy? ParseVacancy(IElement element)
        {
            var titleElement = element.QuerySelector("a.vt-link");
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

            var companyElement = element.QuerySelector("span.vt-company");
            var company = companyElement?.TextContent.Trim();

            // Извлекаем локацию
            var locationElement = element.QuerySelector("span.vt-city");
            var location = locationElement?.TextContent.Trim();

            // Определяем, удалённая ли вакансия (с проверкой на null)
            var isRemote = location?.Contains("Remote", StringComparison.OrdinalIgnoreCase) == true;

            // Извлекаем зарплату
            var salaryElement = element.QuerySelector("span.vt-salary");
            var salaryText = salaryElement?.TextContent.Trim();
            var (salaryMin, salaryMax, currency) = ParseSalary(salaryText);

            // Извлекаем описание
            var descriptionElement = element.QuerySelector("div.vt-description");
            var description = descriptionElement?.TextContent.Trim();

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
                PublishedAt: null);
        }

        private static string MakeAbsoluteUrl(string href)
        {
            if(Uri.TryCreate(href,UriKind.Absolute,out var absoluteUri))
            {
                return absoluteUri.ToString();
            }

            var baseUri = new Uri("https://jobs.dou.ua");

            // 3. Безопасно объединяем базу и относительный путь
            if (Uri.TryCreate(baseUri, href, out var relativeUri))
            {
                return relativeUri.ToString();
            }

            return href;
        }

        private static string ExtractExternalId(string url)
        {
            if(!Uri.TryCreate(url,UriKind.Absolute,out var uri))
            {
                return url;
            }
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return segments.Length > 0 ? segments[^1] : url;
        }
        private static (int? Min, int? Max, string? Currency) ParseSalary(string? salaryText)
        {
            if (string.IsNullOrEmpty(salaryText))
            {
                return (null, null, null);
            }

            // Определяем валюту
            var currency = salaryText.Contains('$') ? "USD"
                         : salaryText.Contains('€') ? "EUR"
                         : salaryText.Contains('₴') ? "UAH"
                         : null;

            // Извлекаем все числа
            var numbers = System.Text.RegularExpressions.Regex
                .Matches(salaryText, @"\d+")
                .Select(m => int.Parse(m.Value))
                .ToList();

            if (numbers.Count == 0)
            {
                return (null, null, currency);
            }

            if (numbers.Count == 1)
            {
                return (numbers[0], numbers[0], currency);
            }

            return (numbers[0], numbers[^1], currency);
        }
    }
}
