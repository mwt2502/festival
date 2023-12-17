using Microsoft.AspNetCore.Mvc;
using festival.Shared.Models;
using festival.Server.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class VolunteerController : ControllerBase
{
    private readonly VolunteerService _volunteerService;

    public VolunteerController(VolunteerService volunteerService)
    {
        _volunteerService = volunteerService;
    }

    // GET: api/volunteer
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Volunteer>>> GetVolunteers()
    {
        var volunteers = await _volunteerService.GetAllAsync();
        return Ok(volunteers);
    }

    // GET: api/volunteer/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Volunteer>> GetVolunteer(string id)
    {
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound();
        }

        return Ok(volunteer);
    }

    // POST: api/volunteer
    [HttpPost]
    public async Task<ActionResult<Volunteer>> PostVolunteer(Volunteer volunteer)
    {
        await _volunteerService.CreateAsync(volunteer);
        return CreatedAtAction("GetVolunteer", new { id = volunteer.Id }, volunteer);
    }

    // PUT: api/volunteer/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVolunteer(string id, Volunteer volunteer)
    {
        if (id != volunteer.Id)
        {
            return BadRequest();
        }

        await _volunteerService.UpdateAsync(id, volunteer);

        return NoContent();
    }

    // DELETE: api/volunteer/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVolunteer(string id)
    {
        var volunteer = await _volunteerService.GetByIdAsync(id);
        if (volunteer == null)
        {
            return NotFound();
        }

        await _volunteerService.RemoveAsync(id);

        return NoContent();
    }
}