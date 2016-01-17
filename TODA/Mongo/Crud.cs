using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODA.Model;

namespace TODA.Mongo
{
    public class Crud
    {
        MongoClient mongoClient;
        private IMongoDatabase database;
        private IMongoCollection<Student> studentsCollection;

        public Crud(string databaseName="shmest", MongoUrl url = null)
        {
            mongoClient = url == null ? new MongoClient() : new MongoClient(url);            
            database = mongoClient.GetDatabase(databaseName);



        }

        public async Task Init()
        {                 
            //await database.CreateCollectionAsync("Students");
            studentsCollection = database.GetCollection<Student>("Students", new MongoCollectionSettings
            {
                AssignIdOnInsert = true,
                WriteConcern = new WriteConcern(0, TimeSpan.FromSeconds(10), false,false)
            });
        }

        public async Task Create(int amount=1000)
        {            
            try
            {                
                
                await studentsCollection.InsertManyAsync(Enumerable.Range(1, amount-1).Select(x => new Student
                {
                    Id = $"Students/{x}",
                    Name = "foo" + x,
                    Age = x % 40,
                    Address = new Address
                    {
                        Appartment = x % 5,
                        City = "City" + x % 4,
                        Country = "Country" + x % 3,
                        House = x % 200
                    }
                }));
            }
            finally
            {

            }
        }

        public async Task Read(int amount = 1000)
        {            
            var filterDefinitionBuilder = new MongoDB.Driver.FilterDefinitionBuilder<Student>();

            for(var i=0; i<amount; i++)
            {
                var filterDefinitino = filterDefinitionBuilder.Eq<string>((Student x) => x.Id , $"Students/{i}");
                var students = await studentsCollection.Find<Student>(filterDefinitino, new FindOptions
                {
                    CursorType = CursorType.NonTailable,


                }).ToListAsync();
                

                if (students.Count ==0 || students.Count !=1 || students[0].Id != $"Students/{i}")
                {
                    throw new Exception("Errorous Read");
                }

            }            
        }
        public async Task OneStepUpdate(int amount = 1000)
        {
            var filterDefinitionBuilder = new FilterDefinitionBuilder<Student>();
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<Student>();
            for (var i = 0; i < amount; i++)
            {                
                await studentsCollection.UpdateOneAsync(
                    filterDefinitionBuilder.Eq<string>((Student x) => x.Id, $"Students/{i}"),
                    updateDefinitionBuilder.Inc<int>((Student x) => x.Age, 1));

            }            
        }

        public async Task TwoStepUpdate(int amount =1000)
        {
            var filterDefinitionBuilder = new FilterDefinitionBuilder<Student>();
            
            for (var i = 0; i < amount; i++)
            {
                var foundStudent = await studentsCollection.Find<Student>((Student x) => x.Id == $"Students/{i}").FirstAsync();                
                foundStudent.Age++;
                await studentsCollection.ReplaceOneAsync(filterDefinitionBuilder.Eq<string>((Student x) => foundStudent.Id, foundStudent.Id), foundStudent);
            }
        }

        public async Task OneStepDelete(int amount = 1000)
        {
            var filterDefinitionBuilder = new FilterDefinitionBuilder<Student>();
            
            for (var i = 0; i < amount; i++)
            {
                var filter = filterDefinitionBuilder.Eq((Student x) => x.Id, $"Students/{i}");
                await studentsCollection.DeleteOneAsync(filter);                
            }
        }        
    }
}
