using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatApi.Services.Interfaces;

namespace WatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController : ControllerBase
    {
        private readonly WatApi.Services.Interfaces.IJobOfferService _service;

        public JobOfferController(IJobOfferService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobOffer([FromBody] WatApi.DTO.JobOffer.JobOfferCreateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                var jobOffer = await _service.CreateJobOfferAsync(userId, dto);
                return StatusCode(201, jobOffer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
