using WatApi.DTO.ImportantInfo;
using WatApi.Models;
namespace WatApi.Services.Interfaces
{
    public interface IImportantInfoService
    {
        Task<ImportantInfo?> GetImportantInfoByJobOfferIdAsync(Guid jobOfferId, Guid userId);
        Task<ImportantInfo> CreateImportantInfoAsync(Guid userId, Guid offerId,
            ImportantInfoCreateDto dto);
        Task<ImportantInfo> UpdateImportantInfoAsync(Guid userId, Guid jobOfferId,
            ImportantInfoUpdateDto dto);
    }
}
