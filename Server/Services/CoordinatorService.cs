using festival.Server.DataService;
using festival.Server.Interfaces;
using festival.Shared.Models;
using MongoDB.Driver;

namespace festival.Server.Services
{
    public class CoordinatorService : ICoordinatorService
    {
        private readonly IMongoCollection<Coordinator> _coordinators;

        public CoordinatorService(MongoDbContext dbContext)
        {
            _coordinators = dbContext.Coordinators;
        }

        // Hent alle coordinators
        public async Task<List<Coordinator>> GetAllAsync()
        {
            return await _coordinators.Find(coordinator => true).ToListAsync();
        }

        // Hent en coordinator ved ID
        public async Task<Coordinator> GetByIdAsync(string id)
        {
            return await _coordinators.Find(coordinator => coordinator.Id == id).FirstOrDefaultAsync();
        }

        // Opret en ny coordinator
        public async Task CreateAsync(Coordinator coordinator)
        {
            if (coordinator == null)
            {
                throw new ArgumentNullException(nameof(coordinator));
            }
            await _coordinators.InsertOneAsync(coordinator);
        }

        // Opdater en coordinator
        public async Task UpdateAsync(string id, Coordinator coordinatorIn)
        {
            await _coordinators.ReplaceOneAsync(coordinator => coordinator.Id == id, coordinatorIn);
        }

        // Slet en coordinator
        public async Task DeleteAsync(string id)
        {
            await _coordinators.DeleteOneAsync(coordinator => coordinator.Id == id);
        }
    }
}
