using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODA.Model;

namespace TODA.Mongo
{
    public class Crud:IDisposable
    {
        MongoClient mongoClient;
        private IMongoDatabase database;

        public Crud(string databaseName="test", MongoUrl url = null)
        {
            mongoClient = url == null ? new MongoClient() : new MongoClient(url);
            database = mongoClient.GetDatabase(databaseName);
            database.CreateCollectionAsync("Students").Wait();
        }

        public void InsertStudents(int amount=1000)
        {            
            try
            {                
                var studentsCollection = database.GetCollection<Student>("Students", new MongoCollectionSettings
                {
                    AssignIdOnInsert = true,
                    WriteConcern = new WriteConcern(0,TimeSpan.FromSeconds(10),true,true)
                });
                studentsCollection.InsertManyAsync(Enumerable.Range(1, amount).Select(x => new Student
                {
                    Name = "foo" + x,
                    Age = x % 40,
                    Address = new Address
                    {
                        Appartment = x % 15,
                        City = "City" + x % 4,
                        Country = "Country" + x % 3,
                        House = x % 200
                    }
                })).Wait();
            }
            finally
            {

            }
        }
        
        public void AgeStudents(int delta)
        {
            var studentsCollection = database.GetCollection<Student>("Students", new MongoCollectionSettings
            {
                AssignIdOnInsert = true,
                WriteConcern = new WriteConcern(1, TimeSpan.FromSeconds(10), true, true)
            })
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
