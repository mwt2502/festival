using Microsoft.AspNetCore.Mvc;
using festival.Shared.Models;
using festival.Server.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using festival.Server.Interfaces;

[ApiController] // Dette angiver, at dette er en API-controller.
[Route("api/[controller]")]
public class VolunteerController : ControllerBase
{
    private readonly IVolunteerService _volunteerService;

    public VolunteerController(IVolunteerService volunteerService)
    {
        _volunteerService = volunteerService;
    }

    // GET: api/Volunteer
    [HttpGet]
    public async Task<ActionResult<List<Volunteer>>> GetVolunteers()
    {
        // Metoden til at hente alle frivillige og returnere dem som ActionResult.
        return await _volunteerService.GetAllAsync();
    }

    // GET: api/Volunteer/5
    [HttpGet("{id:length(24)}", Name = "GetVolunteer")]
    public async Task<ActionResult<Volunteer>> GetVolunteer(string id)
    {
        // Metoden til at hente en enkelt frivillig baseret på id og returnere dem som ActionResult.
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound(); // Hvis frivilligen ikke findes, returneres en NotFound-response.
        }

        return volunteer;
    }

    // POST: api/Volunteer
    [HttpPost]
    public async Task<ActionResult<Volunteer>> CreateVolunteer(Volunteer volunteer)
    {
        // Metoden til at oprette en ny frivillig og returnere dem som ActionResult.
        await _volunteerService.CreateAsync(volunteer);

        return CreatedAtRoute("GetVolunteer", new { id = volunteer.Id.ToString() }, volunteer);
        // Returnerer en CreatedAtRoute-response med en henvisning til GetVolunteer-metoden for den oprettede frivillig.
    }

    // PUT: api/Volunteer/5
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateVolunteer(string id, Volunteer updatedVolunteer)
    {
        // Metoden til at opdatere en eksisterende frivillig baseret på id.
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound(); // Hvis frivilligen ikke findes, returneres en NotFound-response.
        }

        await _volunteerService.UpdateAsync(id, updatedVolunteer);

        return NoContent(); // Returnerer en NoContent-response, når opdateringen er fuldført.
    }

    // DELETE: api/Volunteer/5
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteVolunteer(string id)
    {
        // Metoden til at slette en frivillig baseret på id.
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound(); // Hvis frivilligen ikke findes, returneres en NotFound-response.
        }

        await _volunteerService.DeleteAsync(volunteer.Id);

        return NoContent(); // Returnerer en NoContent-response, når sletningen er fuldført.
    }

    [HttpPost("unassign/{shiftId}")]
    public async Task<IActionResult> UnassignShift(string shiftId)
    {
        bool success = false;

        if (success)
        {
            return Ok("Du er nu afmeldt fra vagten."); // Returnerer en OK-response med en succesmeddelelse.
        }
        else
        {
            return BadRequest("Fejl under afmelding fra vagten."); // Returnerer en BadRequest-response ved fejl.
        }
    }
}
