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
            var userId = Guid.Parse(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var jobOffer = await _service.CreateJobOfferAsync(userId, dto);

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

            return StatusCode(201, jobOfferResponse);
        }

        [HttpGet("GetJoByID")]
        public async Task<IActionResult> GetJobOffer(Guid offerId)
        {
            var jobOffer = await _service.GetJobOfferByIdAsync(offerId);

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

        [HttpGet("GetAllJo")]
        public async Task<IActionResult> GetAllJobOffers()
        {
            var jobOffers = await _service.GetAllJobOffersAsync(Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            var jobOfferResponses = jobOffers.Select(jo => new JobOfferResponseDto
            {
                Id = jo.Id,
                Position = jo.Position,
                Employer = jo.Employer,
                PlaceOfWork = jo.PlaceOfWork,
                PayPerHour = jo.PayPerHour,
                Status = jo.Status,
                HousingProvided = jo.HousingProvided,
                HousingCostPerWeek = jo.HousingCostPerWeek
            }).ToList();
            return Ok(jobOfferResponses);
        }

        [HttpPut("UpdateJo")]
        public async Task<IActionResult> UpdateJobOffer(Guid offerId, [FromBody] JobOfferUpdateDto dto)
        {
                var userId = Guid.Parse(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                var updatedOffer = await _service.UpdateJobOfferAsync(offerId, userId, dto);

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
                return Ok(jobOfferResponse);
        }

        [HttpDelete("DeleteJo")]
        public async Task<IActionResult> DeleteJobOffer(Guid id)
        {
            await _service.DeleteJobOfferAsync(id);
            return NoContent();
        }
    }
}
