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

            await _context.JobOffers.AddAsync(jobOffer);
            _context.SaveChanges();
            return jobOffer;
        }
    }
}
