using Microsoft.AspNetCore.Mvc;
using festival.Shared.Models;
using festival.Server.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using festival.Server.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class VolunteerController : ControllerBase
{
    private readonly IVolunteerService _volunteerService;

    public VolunteerController(IVolunteerService volunteerService)
    {
        _volunteerService = volunteerService;
    }
    // GET: Volunteer
    [HttpGet]
    public async Task<ActionResult<List<Volunteer>>> GetVolunteers()
    {
        return await _volunteerService.GetAllAsync();
    }

    // GET: Volunteer/5
    [HttpGet("{id:length(24)}", Name = "GetVolunteer")]
    public async Task<ActionResult<Volunteer>> GetVolunteer(string id)
    {
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound();
        }

        return volunteer;
    }

    // POST: Volunteer
    [HttpPost]
    public async Task<ActionResult<Volunteer>> CreateVolunteer(Volunteer volunteer)
    {
        await _volunteerService.CreateAsync(volunteer);

        return CreatedAtRoute("GetVolunteer", new { id = volunteer.Id.ToString() }, volunteer);
    }

    // PUT: Volunteer/5
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateVolunteer(string id, Volunteer updatedVolunteer)
    {
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound();
        }

        await _volunteerService.UpdateAsync(id, updatedVolunteer);

        return NoContent();
    }

    // DELETE: Volunteer/5
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteVolunteer(string id)
    {
        var volunteer = await _volunteerService.GetByIdAsync(id);

        if (volunteer == null)
        {
            return NotFound();
        }

        await _volunteerService.DeleteAsync(volunteer.Id);

        return NoContent();
 

    }
}
