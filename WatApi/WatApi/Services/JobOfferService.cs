using Microsoft.EntityFrameworkCore;
using WatApi.Data;
using WatApi.DTO.JobOffer;
using WatApi.Models;
using WatApi.Services.Interfaces;

namespace WatApi.Services
{
    public class JobOfferService : IJobOfferService
    {
        private readonly AppDbContext _context;

        public JobOfferService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobOffer> CreateJobOfferAsync(Guid userId, JobOfferCreateDto dto)
        {
            var offer = await _context.JobOffers.FirstOrDefaultAsync(jo => jo.UserId == userId);
            if (offer != null)
                throw new InvalidOperationException("This user already has a job offer.");

            var jobOffer = new JobOffer()
            {
                UserId = userId,
                Position = dto.Position,
                Employer = dto.Employer,
                PlaceOfWork = dto.PlaceOfWork,
                PayPerHour = dto.PayPerHour,
                HousingProvided = dto.HousingProvided,
                HousingCostPerWeek = dto.HousingCostPerWeek,
            };

            _context.JobOffers.Add(jobOffer);
            await _context.SaveChangesAsync();
            return jobOffer;
        }

        public async Task DeleteJobOfferAsync(Guid id)
        {
            var offer = await _context.JobOffers.FindAsync(id);
            if (offer == null)
                throw new KeyNotFoundException("Job offer not found for the user.");

            _context.JobOffers.Remove(offer);
            await _context.SaveChangesAsync();
        }

        public async Task<JobOffer?> GetJobOfferByUserIdAsync(Guid userId)
        {
            var offer = await _context.JobOffers.FirstOrDefaultAsync(jo => jo.UserId == userId);
            return offer ?? throw new KeyNotFoundException("Job offer not found for the user.");
        }

        public async Task<JobOffer> UpdateJobOfferAsync(Guid userId, JobOfferUpdateDto dto)
        {
            var offer = await GetJobOfferByUserIdAsync(userId) ?? 
                throw new InvalidOperationException("Job offer not found for the user.");

            offer.Position = dto.Position;
            offer.Employer = dto.Employer;
            offer.PlaceOfWork = dto.PlaceOfWork;
            offer.PayPerHour = dto.PayPerHour;
            offer.HousingProvided = dto.HousingProvided;
            offer.HousingCostPerWeek = dto.HousingCostPerWeek;
            offer.Status = dto.Status;

            await _context.SaveChangesAsync();
            return offer;
        }
    }
}
