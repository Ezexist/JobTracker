using FluentValidation;

namespace JobTracker.Application.Features.Vacancies.Create
{
    public sealed class CreateVacancyValidator : AbstractValidator<CreateVacancyCommand>
    {
        public CreateVacancyValidator()
        {
            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Source is empty")
                .MaximumLength(50).WithMessage("Source must not exceed 50 characters");

             RuleFor(x => x.ExternalId)
                .NotEmpty().WithMessage("ExternalId is required")
                .MaximumLength(200).WithMessage("ExternalId must not exceed 200 characters");

             RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(300).WithMessage("Title must not exceed 300 characters");

            RuleFor(x => x.Company)
                .MaximumLength(200).WithMessage("Company must not exceed 200 characters")
                .When(x => x.Company is not null);


            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters")
                .When(x => x.Location is not null);

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url is required")
                .MaximumLength(2048).WithMessage("Url must not exceed 2048 characters");

            RuleFor(x => x.Currency)
                .MaximumLength(3).WithMessage("Currency must not exceed 3 characters")
                .When(x => x.Currency is not null);

            RuleFor(x => x.SalaryMin)
                .GreaterThan(0).WithMessage("Min Salary must be greater than 0")
                .When(x => x.SalaryMin.HasValue);
            RuleFor(x => x.SalaryMax)
                .GreaterThan(0).WithMessage("SalaryMax must be greater than 0")
                .When(x => x.SalaryMax.HasValue);
        }
    }
}
