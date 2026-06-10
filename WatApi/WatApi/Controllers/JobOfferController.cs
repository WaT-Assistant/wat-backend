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

            var jobOfferResponse = new JobOfferResponseDto
            {
                Id = jobOffer.Id,
                Position = jobOffer.Position,
                Employer = jobOffer.Employer,
                PlaceOfWork = jobOffer.PlaceOfWork,
                PayPerHour = jobOffer.PayPerHour,
                Status = jobOffer.Status,
                HousingProvided = jobOffer.HousingProvided,
                HousingCostPerWeek = jobOffer.HousingCostPerWeek,
                Year = jobOffer.Year,
                IsPublished = jobOffer.IsPublished,
                Feedback = jobOffer.Feedback,
                Rating = jobOffer.Rating
            };

            return StatusCode(201, jobOfferResponse);
        }

        [HttpGet("{offerId}")]
        public async Task<IActionResult> GetJobOffer(Guid offerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var jobOffer = await _service.GetJobOfferByIdAsync(offerId, userId);

            var jobOfferResponse = new JobOfferResponseDto
            {
                Id = jobOffer.Id,
                Position = jobOffer.Position,
                Employer = jobOffer.Employer,
                PlaceOfWork = jobOffer.PlaceOfWork,
                PayPerHour = jobOffer.PayPerHour,
                Status = jobOffer.Status,
                HousingProvided = jobOffer.HousingProvided,
                HousingCostPerWeek = jobOffer.HousingCostPerWeek,
                Year = jobOffer.Year,
                IsPublished = jobOffer.IsPublished,
                Feedback = jobOffer.Feedback,
                Rating = jobOffer.Rating
            };

            return Ok(jobOfferResponse);
        }

        [HttpGet]
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
                HousingCostPerWeek = jo.HousingCostPerWeek,
                Year = jo.Year,
                IsPublished = jo.IsPublished,
                Feedback = jo.Feedback,
                Rating = jo.Rating
            }).ToList();
            return Ok(jobOfferResponses);
        }

        [HttpPut("{offerId}")]
        public async Task<IActionResult> UpdateJobOffer(Guid offerId, [FromBody] JobOfferUpdateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
                HousingCostPerWeek = updatedOffer.HousingCostPerWeek,
                Year = updatedOffer.Year,
                IsPublished = updatedOffer.IsPublished,
                Feedback = updatedOffer.Feedback,
                Rating = updatedOffer.Rating
            };
            return Ok(jobOfferResponse);
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

            var jobOfferResponse = new JobOfferResponseDto
            {
                Id = publishedOffer.Id,
                Position = publishedOffer.Position,
                Employer = publishedOffer.Employer,
                PlaceOfWork = publishedOffer.PlaceOfWork,
                PayPerHour = publishedOffer.PayPerHour,
                Status = publishedOffer.Status,
                HousingProvided = publishedOffer.HousingProvided,
                HousingCostPerWeek = publishedOffer.HousingCostPerWeek,
                Year = publishedOffer.Year,
                IsPublished = publishedOffer.IsPublished,
                Feedback = publishedOffer.Feedback,
                Rating = publishedOffer.Rating
            };

            return Ok(jobOfferResponse);
        }
    }
}
