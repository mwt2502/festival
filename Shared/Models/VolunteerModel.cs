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
        public string Id { get; set; }

        [Required(ErrorMessage = "Navn er påkrævet")]
        public string Name { get; set; }
        public List<ObjectId> AssignedShifts { get; set; } = new List<ObjectId>(); // Referencer til Shifts

        public void AssignShift(ObjectId shiftId)
        {
            AssignedShifts.Add(shiftId);

        }

        public void UnassignShift(ObjectId shiftId)
        {
            AssignedShifts.Remove(shiftId);
        }
    }
}
