using festival.Server.DataService;
using festival.Server.Interfaces;
using festival.Shared.Models;
using MongoDB.Driver;

namespace festival.Server.Services
{
    public class VolunteerService: IVolunteerService
    {
        private readonly IMongoCollection<Volunteer> _volunteers;

        public VolunteerService(MongoDbContext context)
        {
            _volunteers = context.Volunteers;
        }

        // Hent alle volunteers
        public async Task<List<Volunteer>> GetAllAsync()
        {
            return await _volunteers.Find(v => true).ToListAsync();
        }

        // Hent en specifik volunteer via ID
        public async Task<Volunteer> GetByIdAsync(string id)
        {
            return await _volunteers.Find<Volunteer>(volunteer => volunteer.Id == id).FirstOrDefaultAsync();
        }

        // Opret en ny volunteer
        public async Task CreateAsync(Volunteer volunteer)
        {
            await _volunteers.InsertOneAsync(volunteer);
        }

        // Opdater en eksisterende volunteer
        public async Task UpdateAsync(string id, Volunteer updatedVolunteer)
        {
            await _volunteers.ReplaceOneAsync(volunteer => volunteer.Id == id, updatedVolunteer);
        }

        // Slet en volunteer
        public async Task DeleteAsync(string id)
        {
            await _volunteers.DeleteOneAsync(volunteer => volunteer.Id == id);
        }
    }
}
