using festival.Server.Interfaces;
using festival.Server.Services;
using festival.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace festival.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Shift>>> Get()
        {
            return Ok(await _shiftService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> Get(string id)
        {
            var shift = await _shiftService.GetByIdAsync(id);   

            if (shift == null)
            {
                return NotFound();
            }
            return Ok(shift);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Shift shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validationMessage = shift.ValidateTimes();
            if (!string.IsNullOrEmpty(validationMessage))
            {
                // Hvis der er en valideringsfejl, returner den som BadRequest
                return BadRequest(validationMessage);
            }

            await _shiftService.CreateAsync(shift);

            // Returnerer et CreatedAtAction svar med den nye shift
            return CreatedAtAction(nameof(Get), new { id = shift.Id }, shift);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Shift shift)
        {
            var existingShift = await _shiftService.GetByIdAsync(id);
            if (existingShift == null)
            {
                return NotFound();
            }

            await _shiftService.UpdateAsync(id, shift);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var shift = await _shiftService.GetByIdAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            await _shiftService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPut("{shiftId}/assign")]
        public async Task<IActionResult> AssignVolunteerToShift(string shiftId, [FromBody] AssignVolunteerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _shiftService.AssignVolunteer(shiftId, dto.VolunteerId);
                return NoContent(); // 204 No Content hvis det lykkes
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public class AssignVolunteerDto
        {
            [Required]
            public string VolunteerId { get; set; }
        }
    }

}
