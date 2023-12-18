using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace festival.Shared.Models
{
    public class Shift
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
             public string? Id { get; set; }

        public string? Title { get; set; } // tilføj title til form og fjern ?

        [BsonRepresentation(BsonType.String)]
                 public ShiftImportance Importance { get; set; }

        [BsonRepresentation(BsonType.String)]
                public ShiftArea Area { get; set; }

        [Required]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage =
            "Starttid skal være i format TT:mm")]
                public string? StartTime { get; set; }

        [Required]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = 
            "Sluttid skal være i format TT:mm")]
                public string? EndTime { get; set; }

        public int RequiredVolunteers { get; set; }
        public int AssignedVolunteers { get; set; }

        /* public ObjectId? AssignedVolunteerId { get; set; } // Referencer til en Volunteer */
        public bool IsFull => AssignedVolunteers >= RequiredVolunteers;

        // Server validering af datoer 
        public bool ValidateTimes()
        {
            var timeRegex = new Regex(@"^(?:[01]\d|2[0-3]):[0-5]\d$");
            return timeRegex.IsMatch(StartTime) && timeRegex.IsMatch(EndTime);
        }
        public enum ShiftImportance
        {
            [EnumMember(Value = "Low")]
            Low,
            [EnumMember(Value = "Medium")]
            Medium,
            [EnumMember(Value = "High")]
            High
        }
        public enum ShiftArea
        {
            Madbod_Kebab,
            Madbod_Burger,
            Madbod_Sushi,
            Hoppeoborg,
            Indgang,
            Udgang,
            Scene_1,
            Scene_2,
            Scene_3,
        }

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
