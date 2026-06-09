using Microsoft.EntityFrameworkCore;
using WatApi.Data;
using WatApi.DTO.ImportantInfo;
using WatApi.Models;
using WatApi.Services.Interfaces;

namespace WatApi.Services
{
    public class ImportantInfoService(AppDbContext context) : IImportantInfoService
    {
        private readonly AppDbContext _context = context;

        public async Task<ImportantInfo> CreateImportantInfoAsync(Guid userId, Guid offerId, ImportantInfoCreateDto dto)
        {
            var offer = await _context.JobOffers.Include(jo => jo.ImportantInfo).FirstOrDefaultAsync(jo => jo.Id == offerId)
              ?? throw new KeyNotFoundException("Job offer not found");

            if (offer.ImportantInfo != null)
                throw new InvalidOperationException("Important info already exists for this job offer");
            if (offer.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to add important info to this job offer");

            var importantInfo = new ImportantInfo
            {
                Id = Guid.NewGuid(),
                JobOfferId = offerId,
                SevisID = dto.SevisId,
                VisaAppointment = dto.VisaAppointment,
                Flight = dto.FlightDate,
                DS160 = dto.Ds160,
                DS2019 = dto.Ds2019
            };

            _context.ImportantInfos.Add(importantInfo);
            await _context.SaveChangesAsync();

            return importantInfo;
        }

        public async Task<ImportantInfo> GetImportantInfoByJobOfferIdAsync(Guid jobOfferId)
        {
            throw new NotImplementedException();
        }

        public async Task<ImportantInfo> UpdateImportantInfoAsync(Guid userId, Guid jobOfferId,
            ImportantInfoUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
