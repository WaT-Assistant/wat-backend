using FluentValidation;
using WatApi.DTO.JobOffer;

namespace WatApi.Validators.JobOffer
{
    public class JobOfferCreateDtoValidator: AbstractValidator<JobOfferCreateDto>
    {
        public JobOfferCreateDtoValidator()
        {
            RuleFor(x => x.Position).NotEmpty().WithMessage("Position field can't be empty!");
            RuleFor(x => x.Employer).NotEmpty().WithMessage("Employer field can't be empty!");

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
