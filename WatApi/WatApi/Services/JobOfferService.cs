using Microsoft.EntityFrameworkCore;
using WatApi.Data;
using WatApi.DTO.JobOffer;
using WatApi.Models;
using WatApi.Services.Interfaces;

namespace WatApi.Services
{
    public class JobOfferService(AppDbContext context) : IJobOfferService
    {
        private readonly AppDbContext _context = context;

        public async Task<JobOffer> CreateJobOfferAsync(Guid userId, JobOfferCreateDto dto)
        {
            var jobOffer = new JobOffer()
            {
                UserId = userId,
                Position = dto.Position,
                Employer = dto.Employer,
                PlaceOfWork = dto.PlaceOfWork,
                PayPerHour = dto.PayPerHour,
                HousingProvided = dto.HousingProvided,
                HousingCostPerWeek = dto.HousingCostPerWeek,
                Year = dto.Year,
            };

            _context.JobOffers.Add(jobOffer);
            await _context.SaveChangesAsync();
            return jobOffer;
        }

        public async Task DeleteJobOfferAsync(Guid id)
        {
            var offer = await _context.JobOffers.FindAsync(id) ?? 
                throw new KeyNotFoundException("Job offer not found for the user.");

            _context.JobOffers.Remove(offer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobOffer>> GetAllJobOffersAsync(Guid userId) =>
            await _context.JobOffers
        .Where(jo => jo.UserId == userId)
        .ToListAsync();

        public async Task<JobOffer> GetJobOfferByIdAsync(Guid offerId)
        {
            var offer = await _context.JobOffers.FindAsync(offerId);
            return offer ?? throw new KeyNotFoundException("Job offer not found.");
        }

        public async Task<JobOffer> PublishJobOfferAsync(Guid offerId, Guid userId, JobOfferPublishDto dto)
        {
            var offer = await GetJobOfferByIdAsync(offerId);
            if (offer.UserId != userId)
                throw new UnauthorizedAccessException
                    ("You do not have permission to publish this job offer.");
            if(offer.IsPublished)
                throw new InvalidOperationException("This job offer is already published.");

            offer.IsPublished = true;
            offer.Feedback = dto.Feedback;
            offer.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<JobOffer> UpdateJobOfferAsync(Guid offerId, Guid userId, JobOfferUpdateDto dto)
        {
            var offer = await GetJobOfferByIdAsync(offerId);
            if (offer.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to update this job offer.");

            offer.Position = dto.Position;
            offer.Employer = dto.Employer;
            offer.PlaceOfWork = dto.PlaceOfWork;
            offer.PayPerHour = dto.PayPerHour;
            offer.HousingProvided = dto.HousingProvided;
            offer.HousingCostPerWeek = dto.HousingCostPerWeek;
            offer.Status = dto.Status;
            offer.Year = dto.Year;

            await _context.SaveChangesAsync();
            return offer;
        }
    }
}
