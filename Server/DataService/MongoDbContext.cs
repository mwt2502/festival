namespace festival.Server.DataService
{
    using festival.Shared.Models;
    using MongoDB.Driver;

    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<Volunteer> Volunteers { get; }
        public IMongoCollection<Coordinator> Coordinators { get; }
        public IMongoCollection<Shift> Shifts { get; }

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value;
            var client = new MongoClient(connectionString);
            var databaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value;

            _database = client.GetDatabase(databaseName);

            // Initialiser collections
            Volunteers = _database.GetCollection<Volunteer>("Volunteers");
            Coordinators = _database.GetCollection<Coordinator>("Coordinators");
            Shifts = _database.GetCollection<Shift>("Shifts");
        }
    }

}
