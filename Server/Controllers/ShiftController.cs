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

        [HttpPut("{shiftId}/assign/{volunteerId}")]
        public async Task<IActionResult> xx(string shiftId, string volunteerId)
        {
            try
            {
                await _shiftService.AssignVolunteer(shiftId, volunteerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{shiftId}/unassign/{volunteerId}")]
        public async Task<IActionResult> UnassignVolunteer(string shiftId, string volunteerId)
        {
            bool success = await _shiftService.UnassignVolunteer(shiftId, volunteerId);
            if (success)
            {
                return Ok("Volunteer unassigned successfully.");
            }
            else
            {
                return BadRequest("Error unassigning volunteer.");
            }
        }

        [HttpGet("assignedTo/{volunteerId}")]
        public async Task<IActionResult> GetAssignedShifts(string volunteerId)
        {
            try
            {
                var assignedShifts = await _shiftService.GetAssignedShiftsAsync(volunteerId);

                // Returner vagterne som JSON-respons
                return Ok(assignedShifts);
            }
            catch (Exception ex)
            {
                // Hvis der opstår en fejl, kan du returnere en fejlrespons med passende statuskode og meddelelse
                return StatusCode(500, $"An error occurred: {ex.Message} (controller)");
            }
        }


    }

}
