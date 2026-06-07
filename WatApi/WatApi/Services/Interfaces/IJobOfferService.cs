using WatApi.DTO.JobOffer;
using WatApi.Models;

namespace WatApi.Services.Interfaces
{
    public interface IJobOfferService
    {
        Task<JobOffer> CreateJobOfferAsync(Guid userId, JobOfferCreateDto dto);
        Task<JobOffer?> GetJobOfferByUserIdAsync(Guid userId);
        Task<JobOffer> UpdateJobOfferAsync(Guid userId, JobOfferUpdateDto dto);
    }
}
