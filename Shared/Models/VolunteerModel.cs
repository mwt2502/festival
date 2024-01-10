using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace festival.Shared.Models
{
    public class Volunteer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Unik identifikator for en frivillig, repræsenteret som en ObjectId fra MongoDB

        [Required(ErrorMessage = "Navn er påkrævet")]
        public string? Name { get; set; } // Navnet på frivilligen, markeret som påkrævet med en fejlbesked ved manglende udfyldelse

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> AssignedShifts { get; set; } = new List<string>(); // En liste over strengrepræsentationer af tildelte vagter


    }
}
