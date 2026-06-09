using WatApi.DTO.JobOffer;
using WatApi.Models;

namespace WatApi.Services.Interfaces
{
    public interface IJobOfferService
    {
        Task<JobOffer> CreateJobOfferAsync(Guid userId, JobOfferCreateDto dto);
        Task<JobOffer> GetJobOfferByIdAsync(Guid offerId);
        Task<IEnumerable<JobOffer>> GetAllJobOffersAsync(Guid userId);
        Task<JobOffer> UpdateJobOfferAsync(Guid offerId, Guid userId, JobOfferUpdateDto dto);
        Task DeleteJobOfferAsync(Guid id);
        Task<JobOffer> PublishJobOfferAsync(Guid offerId, Guid userId, JobOfferPublishDto dto);
    }
}
