using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODA.Model;

namespace TODA.Mongo
{
    public class Patch
    {
        private IMongoDatabase database;
        private MongoClient mongoClient;

        public Patch(string databaseName = "test", MongoUrl url = null)
        {
            mongoClient = url == null ? new MongoClient() : new MongoClient(url);
            database = mongoClient.GetDatabase(databaseName);
            
        }

        public async void Init()
        {
            await database.CreateCollectionAsync("Students");
        }
        public async void UpdateBatch(int amount = 1000)
        {
            var studentsCollection = database.GetCollection<Student>("Students");
            var filterDefinitionBuilder = new MongoDB.Driver.FilterDefinitionBuilder<Student>();


            var updateDefinitionBuilder = new UpdateDefinitionBuilder<Student>();
            var updateDefinition = updateDefinitionBuilder.Inc(x => x.Age, 1);
            await studentsCollection.UpdateManyAsync(new FilterDefinitionBuilder<Student>().Exists((Student x) => x.Id, true), updateDefinition);
            for (var i = 0; i < amount; i++)
            {
                var filterDefinitino = filterDefinitionBuilder.Eq<string>((Student x) => x.Id, $"Students/{i}");
                var students = await studentsCollection.Find<Student>(filterDefinitino, new FindOptions
                {
                    CursorType = CursorType.NonTailable,


                }).ToListAsync();
                                
                if (students.Count == 0 || students.Count != 1 || students[0].Id != $"Students/{i}")
                {
                    throw new Exception("Errorous Read");
                }

            }
        }
    }
}
