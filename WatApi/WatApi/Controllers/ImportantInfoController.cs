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

        [HttpPost("{offerId}/CreateIi")]
        public async Task<IActionResult> CreateImportantInfo(Guid offerId, [FromBody] ImportantInfoCreateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var createdInfo = await _importantInfoService.CreateImportantInfoAsync(userId, offerId, dto);

            var response = new ImportantInfoResponseDto
            {
                SevisId = createdInfo.SevisID,
                VisaAppointment = createdInfo.VisaAppointment,
                FlightDate = createdInfo.Flight,
                Ds160 = createdInfo.DS160,
                Ds2019 = createdInfo.DS2019
            };

            return Ok(response);
        }

        [HttpGet("{offerId}/GetIiById")]
        public async Task<IActionResult> GetImportantInfoByOfferId(Guid offerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var importantInfo = await _importantInfoService.
                GetImportantInfoByJobOfferIdAsync(offerId, userId);

            if (importantInfo == null) return NoContent();

            var response = new ImportantInfoResponseDto
            {
                SevisId = importantInfo.SevisID,
                VisaAppointment = importantInfo.VisaAppointment,
                FlightDate = importantInfo.Flight,
                Ds160 = importantInfo.DS160,
                Ds2019 = importantInfo.DS2019
            };

            return Ok(response);
        }
    }
}
