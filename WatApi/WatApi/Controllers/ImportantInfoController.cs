using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatApi.DTO.ImportantInfo;
using WatApi.Services.Interfaces;

namespace WatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImportantInfoController(IImportantInfoService importantInfoService) : ControllerBase
    {
        private readonly IImportantInfoService _importantInfoService = importantInfoService;

        [HttpPost("{offerId}")]
        public async Task<IActionResult> CreateImportantInfo(Guid offerId, [FromBody] ImportantInfoCreateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var createdInfo = await _importantInfoService.CreateImportantInfoAsync(userId, offerId, dto);

            var response = new ImportantInfoResponseDto
            {
                Id = createdInfo.Id,
                SevisId = createdInfo.SevisID,
                VisaAppointment = createdInfo.VisaAppointment,
                FlightDate = createdInfo.Flight,
                Ds160 = createdInfo.DS160,
                StartOfWork = createdInfo.StartOfWork,
                EndOfWork = createdInfo.EndOfWork
            };

            return Ok(response);
        }

        [HttpGet("{offerId}")]
        public async Task<IActionResult> GetImportantInfoByOfferId(Guid offerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var importantInfo = await _importantInfoService.
                GetImportantInfoByJobOfferIdAsync(offerId, userId);

            if (importantInfo == null) return NoContent();

            var response = new ImportantInfoResponseDto
            {
                Id = importantInfo.Id,
                SevisId = importantInfo.SevisID,
                VisaAppointment = importantInfo.VisaAppointment,
                FlightDate = importantInfo.Flight,
                Ds160 = importantInfo.DS160,
                StartOfWork= importantInfo.StartOfWork,
                EndOfWork = importantInfo.EndOfWork
            };

            return Ok(response);
        }

        [HttpPut("{offerId}")]
        public async Task<IActionResult> UpdateImportantInfoByOfferId(Guid offerId, [FromBody] ImportantInfoUpdateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var updatedInfo = await _importantInfoService.UpdateImportantInfoAsync(userId, offerId, dto);

            var response = new ImportantInfoResponseDto
            {
                Id = updatedInfo.Id,
                SevisId = updatedInfo.SevisID,
                VisaAppointment = updatedInfo.VisaAppointment,
                FlightDate = updatedInfo.Flight,
                Ds160 = updatedInfo.DS160,
                StartOfWork = updatedInfo.StartOfWork,
                EndOfWork = updatedInfo.EndOfWork
            };

            return Ok(response);
        }

        [HttpDelete("${id}")]
        public async Task<IActionResult> DeleteImportantInfoById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _importantInfoService.DeleteImportantInfoAsync(id, userId);

            return NoContent();
        }
    }
}
