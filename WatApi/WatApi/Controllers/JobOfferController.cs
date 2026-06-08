using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatApi.DTO.JobOffer;
using WatApi.Models;
using WatApi.Services.Interfaces;

namespace WatApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController(IJobOfferService service) : ControllerBase
    {
        private readonly IJobOfferService _service = service;

        [HttpPost("CreateJo")]
        public async Task<IActionResult> CreateJobOffer([FromBody] JobOfferCreateDto dto)
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

        [HttpGet("GetJoByID")]
        public async Task<IActionResult> GetJobOffer()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdString!);
            var jobOffer = await _service.GetJobOfferByUserIdAsync(userId);

            if (jobOffer == null)
                return NotFound("No job offer found for the user.");

            var jobOfferResponse = new JobOfferResponseDto
            {
                Id = jobOffer.Id,
                Position = jobOffer.Position,
                Employer = jobOffer.Employer,
                PlaceOfWork = jobOffer.PlaceOfWork,
                PayPerHour = jobOffer.PayPerHour,
                Status = jobOffer.Status,
                HousingProvided = jobOffer.HousingProvided,
                HousingCostPerWeek = jobOffer.HousingCostPerWeek
            };
            return Ok(jobOfferResponse);
        }

        [HttpPut("UpdateJo")]
        public async Task<IActionResult> UpdateJobOffer([FromBody] JobOfferUpdateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                var updatedOffer = await _service.UpdateJobOfferAsync(userId, dto);
                var jobOfferResponse = new JobOfferResponseDto
                {
                    Id = updatedOffer.Id,
                    Position = updatedOffer.Position,
                    Employer = updatedOffer.Employer,
                    PlaceOfWork = updatedOffer.PlaceOfWork,
                    PayPerHour = updatedOffer.PayPerHour,
                    Status = updatedOffer.Status,
                    HousingProvided = updatedOffer.HousingProvided,
                    HousingCostPerWeek = updatedOffer.HousingCostPerWeek
                };
                return Ok(updatedOffer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteJo")]
        public async Task<IActionResult> DeleteJobOffer(Guid id)
        {
            await _service.DeleteJobOfferAsync(id);
            return NoContent();
        }
    }
}
