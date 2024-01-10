using festival.Server.DataService;
using festival.Server.Interfaces;
using festival.Shared.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Http;

namespace festival.Server.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IMongoCollection<Shift> _shifts;
        private readonly IMongoCollection<Volunteer> _volunteers; 


        public ShiftService(MongoDbContext dbContext)
        {
            var pack = new ConventionPack { new EnumRepresentationConvention(BsonType.String) };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
            _shifts = dbContext.Shifts;
            _volunteers = dbContext.Volunteers; 

        }


        // Hent alle shifts
        public async Task<List<Shift>> GetAllAsync()
        {
            return await _shifts.Find(_ => true).ToListAsync();
        }

        // Hent et specifikt shift ved ID
        public async Task<Shift> GetByIdAsync(string id)
        {
            return await _shifts.Find(shift => shift.Id == id).FirstOrDefaultAsync();
        }

        // Opret et nyt shift
        public async Task CreateAsync(Shift shift)
        {
            if (shift == null)
            {
                throw new ArgumentNullException(nameof(shift));
            }
            await _shifts.InsertOneAsync(shift);
        }

        // Opdater et shift
        public async Task UpdateAsync(string id, Shift updatedShift)
        {
            if (updatedShift == null)
            {
                throw new ArgumentNullException(nameof(updatedShift));
            }
            await _shifts.ReplaceOneAsync(shift => shift.Id == id, updatedShift);
        }

        // Slet et shift
        public async Task DeleteAsync(string id)
        {
            await _shifts.DeleteOneAsync(shift => shift.Id == id);
        }

        //"assign" en frivillig
        public async Task AssignVolunteer(string shiftId, string volunteerId)
        {
            var filter = Builders<Shift>.Filter.Eq("Id", shiftId);
            var update = Builders<Shift>.Update.AddToSet("AssignedVolunteersId", volunteerId);

            var result = await _shifts.UpdateOneAsync(filter, update);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                throw new Exception("Vagt blev ikke opdateret med den frivillige.");
            }
        }
        public async Task<bool> UnassignVolunteer(string shiftId, string volunteerId)
        {
            var filter = Builders<Shift>.Filter.Eq(s => s.Id, shiftId);
            var update = Builders<Shift>.Update.Pull(s => s.AssignedVolunteersId, volunteerId);

            var result = await _shifts.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<List<Shift>> GetAssignedShiftsAsync(string volunteerId)
        {
            // Brug en filterdefinition til at finde vagter, hvor volunteerId er i AssignedVolunteersId
            var filter = Builders<Shift>.Filter.AnyEq(s => s.AssignedVolunteersId, volunteerId);

            // Find og returner de tilmeldte vagter baseret på filteret
            var assignedShifts = await _shifts.Find(filter).ToListAsync();

            return assignedShifts;
        }

    }


}


