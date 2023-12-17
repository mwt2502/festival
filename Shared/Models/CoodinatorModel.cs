using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festival.Shared.Models
{
    public class Coordinator
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }
}
