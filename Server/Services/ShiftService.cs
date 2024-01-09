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

        public ShiftService(MongoDbContext dbContext)
        {
            var pack = new ConventionPack { new EnumRepresentationConvention(BsonType.String) };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
            _shifts = dbContext.Shifts;
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

        public async Task AssignVolunteer(string shiftId, string volunteerId)
        {
            if (!ObjectId.TryParse(shiftId, out var validShiftId))
            {
                throw new ArgumentException("shiftId er ikke en gyldig ObjectId", nameof(shiftId));
            }
            if (!ObjectId.TryParse(volunteerId, out var validVolunteerId))
            {
                throw new ArgumentException("volunteerId er ikke en gyldig ObjectId", nameof(volunteerId));
            }

            var filter = Builders<Shift>.Filter.Eq(nameof(Shift.Id), validShiftId);
            var update = Builders<Shift>.Update.AddToSet(nameof(Shift.AssignedVolunteersId), validVolunteerId);
            var result = await _shifts.UpdateOneAsync(filter, update);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                throw new Exception("Vagt blev ikke opdateret med den frivillige.");
            }
        }



    }

}
