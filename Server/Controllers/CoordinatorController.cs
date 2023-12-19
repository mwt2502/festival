using festival.Server.Services;
using festival.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace festival.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoordinatorController : ControllerBase
    {
        private readonly CoordinatorService _coordinatorService;

        public CoordinatorController(CoordinatorService coordinatorService)
        {
            _coordinatorService = coordinatorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Coordinator>>> Get()
        {
            return await _coordinatorService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Coordinator>> Get(string id)
        {
            var coordinator = await _coordinatorService.GetByIdAsync(id);

            if (coordinator == null)
            {
                return NotFound();
            }

            return coordinator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Coordinator coordinator)
        {
            await _coordinatorService.CreateAsync(coordinator);
            return CreatedAtAction(nameof(Get), new { id = coordinator.Id }, coordinator);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Coordinator coordinatorIn)
        {
            var coordinator = await _coordinatorService.GetByIdAsync(id);

            if (coordinator == null)
            {
                return NotFound();
            }

            await _coordinatorService.UpdateAsync(id, coordinatorIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var coordinator = await _coordinatorService.GetByIdAsync(id);

            if (coordinator == null)
            {
                return NotFound();
            }

            await _coordinatorService.DeleteAsync(id);

            return NoContent();
        }
    }
}
