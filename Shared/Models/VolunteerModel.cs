using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festival.Shared.Models
{
    public class Volunteer
    {
        public ObjectId VolunteerId { get; set; }
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
