using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODA.Model
{
    public class Student
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }        
        public int Age { get; set; }
        public Address Address { get; set; }

    }
    public class Address
    {
        public string City { get; set; }
        public int House { get; set; }
        public int Appartment { get; set; }
        public string Country { get; set; }
    }
}
