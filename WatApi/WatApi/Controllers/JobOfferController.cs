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

        [HttpPost]
        public async Task<IActionResult> CreateJobOffer([FromBody] JobOfferCreateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var jobOffer = await _service.CreateJobOfferAsync(userId, dto);

            return StatusCode(201, MapToDto(jobOffer));
        }

        [HttpGet("{offerId}")]
        public async Task<IActionResult> GetJobOffer(Guid offerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var jobOffer = await _service.GetJobOfferByIdAsync(offerId, userId);

            return Ok(MapToDto(jobOffer));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobOffers()
        {
            var jobOffers = await _service.GetAllJobOffersAsync(Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return Ok(jobOffers.Select(MapToDto));
        }

        [HttpPut("{offerId}")]
        public async Task<IActionResult> UpdateJobOffer(Guid offerId, [FromBody] JobOfferUpdateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var updatedOffer = await _service.UpdateJobOfferAsync(offerId, userId, dto);

            return Ok(MapToDto(updatedOffer));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobOffer(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _service.DeleteJobOfferAsync(id, userId);

            return NoContent();
        }

        [HttpPut("{id}/publish")]
        public async Task<IActionResult> PublishJobOffer(Guid id, [FromBody] 
        JobOfferPublishDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var publishedOffer = await _service.PublishJobOfferAsync(id, userId, dto);
            return Ok(MapToDto(publishedOffer));
        }

        private static JobOfferResponseDto MapToDto(JobOffer offer)
        {
            return new JobOfferResponseDto
            {
                Id = offer.Id,
                Position = offer.Position,
                Employer = offer.Employer,
                PlaceOfWork = offer.PlaceOfWork,
                PayPerHour = offer.PayPerHour,
                Status = offer.Status,
                HousingProvided = offer.HousingProvided,
                HousingCostPerWeek = offer.HousingCostPerWeek,
                Year = offer.Year,
                IsPublished = offer.IsPublished,
                Feedback = offer.Feedback,
                Rating = offer.Rating
            };
        }
    }
}
