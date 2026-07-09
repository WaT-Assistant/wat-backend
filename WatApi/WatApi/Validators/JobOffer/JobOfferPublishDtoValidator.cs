using FluentValidation;
using WatApi.DTO.JobOffer;

namespace WatApi.Validators.JobOffer
{
    public class JobOfferPublishDtoValidator: AbstractValidator<JobOfferPublishDto>
    {
        public JobOfferPublishDtoValidator()
        {
            RuleFor(x => x.Feedback)
                .MaximumLength(300)
                .WithMessage("Feedback should be less than 300 characters!");
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .When(x => x.Rating.HasValue)
                .WithMessage("Rating should be between 1 and 5.");
        }
    }
}
