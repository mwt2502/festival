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

        [Range(1, 50, ErrorMessage = "Antallet af krævede frivillige skal være mellem 1 og 50.")]
        public int RequiredVolunteers { get; set; }
        public int AssignedVolunteers { get; set; }

        public int? AssignedVolunteersId { get; set; } // Referencer til en Volunteer 
        public bool IsFull => AssignedVolunteers >= RequiredVolunteers;

        // Server validering af datoer 
        public string ValidateTimes()
        {
            var timeRegex = new Regex(@"^(?:[01]\d|2[0-3]):[0-5]\d$");
            if (string.IsNullOrWhiteSpace(StartTime) || !timeRegex.IsMatch(StartTime))
            {
                return "Starttid skal være i format TT:mm";
            }

            if (string.IsNullOrWhiteSpace(EndTime) || !timeRegex.IsMatch(EndTime))
            {
                return "Sluttid skal være i format TT:mm";
            }

            TimeSpan start = TimeSpan.Parse(StartTime);
            TimeSpan end = TimeSpan.Parse(EndTime);

            if (end <= start.Add(TimeSpan.FromHours(1)))
            {
                return "Hovsa! Tjek lige tiderne igen. Sluttid skal være mindst en time efter starttiden";
            }

            return string.Empty; // Ingen fejl
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
