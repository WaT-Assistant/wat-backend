using FluentValidation;
using WatApi.DTO.JobOffer;

namespace WatApi.Validators.JobOffer
{
    public class JobOfferCreateDtoValidator: AbstractValidator<JobOfferCreateDto>
    {
        private readonly int _currentYear = new DateOnly().Year;
        public JobOfferCreateDtoValidator()
        {
            RuleFor(x => x.Position).NotEmpty().WithMessage("Position field can't be empty!");
            RuleFor(x => x.Employer).NotEmpty().WithMessage("Employer field can't be empty!");
            RuleFor(x => x.PlaceOfWork).NotEmpty().WithMessage("Location field can't be empty!");
            RuleFor(x => x.PayPerHour).GreaterThan(0)
                .WithMessage("Hourly pay should be greater than zero");
            RuleFor(x => x.Year).InclusiveBetween(_currentYear - 6, _currentYear + 1)
                .WithMessage($"Year should be within {_currentYear - 6} and {_currentYear + 1}");

            RuleFor(x => x.HousingCostPerWeek).GreaterThanOrEqualTo(0)
                .WithMessage("Housing price should be more than or equal to zero");

            RuleFor(x => x.HousingCostPerWeek)
                .NotNull()
                .When(x => x.HousingProvided == true)
                .WithMessage("Housing cost is required when housing is provided.");

            RuleFor(x => x.HousingCostPerWeek)
                .Null()
                .When(x => x.HousingProvided == false)
                .WithMessage("You cannot specify a housing cost if housing is not provided.");
        }
    }
}
