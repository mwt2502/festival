using Microsoft.Extensions.Configuration;

namespace festival.Server.DataService
{
    using festival.Shared.Models;
    using MongoDB.Driver;

    // En klasse, der repræsenterer en databaseforbindelse til MongoDB
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        // Offentlig egenskab for at give adgang til IMongoDatabase-grænsefladen
        public IMongoDatabase Database => _database;

        // Offentlige egenskaber til at få adgang til MongoDB-samlinger
        public IMongoCollection<Volunteer> Volunteers { get; }
        // public IMongoCollection<Coordinator> Coordinators { get; } Ikke brugt i denne version
        public IMongoCollection<Shift> Shifts { get; }

        // Konstruktør, der tager IConfiguration som en parameter for at konfigurere forbindelsen til MongoDB
        public MongoDbContext(IConfiguration configuration)
        {
            // Hent forbindelsesstrengen til MongoDB fra konfigurationsfilerne
            var connectionString = configuration.GetValue<string>("MongoDbSettings:ConnectionString");

            // Opret en MongoClient ved hjælp af forbindelsesstrengen
            var client = new MongoClient(connectionString);

            // Hent database navnet fra konfigurationsfilerne
            var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");

            // Få adgang til den specifikke database ved hjælp af klienten
            _database = client.GetDatabase(databaseName);

            // Initialiser MongoDB-samlinger ved hjælp af de definerede modeller
            Volunteers = _database.GetCollection<Volunteer>("Volunteers");
            // Coordinators = _database.GetCollection<Coordinator>("Coordinators"); // Ikke brugt i denne version
            Shifts = _database.GetCollection<Shift>("Shifts");
        }
    }
}
