using festival.Server.DataService;
using festival.Server.Interfaces;
using festival.Shared.Models;
using MongoDB.Driver;
using System.Data.Entity;

namespace festival.Server.Services
{
    public class VolunteerService: IVolunteerService
    {
        private readonly IMongoCollection<Volunteer> _volunteers;
        private readonly IMongoCollection<Shift> _shifts;


        public VolunteerService(MongoDbContext context)
        {
            _volunteers = context.Volunteers;
            _shifts = context.Database.GetCollection<Shift>("Shifts");

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
        public async Task<Volunteer> CreateAsync(Volunteer volunteer)
        {
            await _volunteers.InsertOneAsync(volunteer);
            return volunteer;

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
        public async Task<List<Shift>> GetAssignedShiftsAsync(string volunteerId)
        {
            // Antager at _shifts er din MongoDB collection af Shifts
            return await _shifts.Find(shift => shift.AssignedVolunteersId.Contains(volunteerId)).ToListAsync();
        }
        public async Task UnassignShiftFromVolunteer(string volunteerId, string shiftId)
        {
            var volunteerFilter = Builders<Volunteer>.Filter.Eq(v => v.Id, volunteerId);
            var volunteerUpdate = Builders<Volunteer>.Update.Pull(v => v.AssignedShifts, shiftId);
            await _volunteers.UpdateOneAsync(volunteerFilter, volunteerUpdate);

        }
        public async Task<bool> AssignShiftToVolunteer(string volunteerId, string shiftId)
        {
            var volunteerFilter = Builders<Volunteer>.Filter.Eq(v => v.Id, volunteerId);
            var volunteerUpdate = Builders<Volunteer>.Update.AddToSet(v => v.AssignedShifts, shiftId);
            var updateResult = await _volunteers.UpdateOneAsync(volunteerFilter, volunteerUpdate);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }


    }
}
