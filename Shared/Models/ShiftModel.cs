using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace festival.Shared.Models
{
    public enum ShiftImportance
    {
        Low,
        Medium,
        High,
    }
    public class Shift
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public ShiftImportance Importance { get; set; }
        public string Area { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RequiredVolunteers { get; set; }
        public int AssignedVolunteers { get; set; }

        /* public ObjectId? AssignedVolunteerId { get; set; } // Referencer til en Volunteer */
        public bool IsFull => AssignedVolunteers >= RequiredVolunteers;

        // Andre egenskaber eller metoder efter behov

        public void AssignVolunteer()
        {
            if (!IsFull)
            {
                AssignedVolunteers++;
                // Yderligere logik efter behov
            }
            else
            {
                // Håndtering af fuldt bemandet vagt
            }
        }

        public void UnassignVolunteer()
        {
            if (AssignedVolunteers > 0)
            {
                AssignedVolunteers--;
                // Yderligere logik efter behov
            }
            else
            {
                // Håndtering af ingen tildelte frivillige
            }
        }
    }
}
