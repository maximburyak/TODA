using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TODA.Model;

namespace TODA.Mongo
{
    public class Querying
    {
        private MongoClient client;
        private IMongoCollection<Student> collection;
        private IMongoDatabase database;

        public Querying()
        {
            client = new MongoClient(); ;
            database = client.GetDatabase("test");

            collection = database.GetCollection<Student>("Users");
        }

        public async void CreateSimpleUniniqueIndex()
        {         
            var indexKeysDefinitionBuilder = new IndexKeysDefinitionBuilder<Student>();
            Expression<Func<Student, String>> userNameExpression = x => x.Name;

            var field = new ExpressionFieldDefinition<Student>(userNameExpression);
            
            var ascendingIndex = indexKeysDefinitionBuilder.Ascending(field);
        
            await collection.Indexes.CreateOneAsync(ascendingIndex, new CreateIndexOptions
            {                
                Name="StudentsNamesUnique",
            });
        }

        public async void QuerySimpleUniniqueIndex(string value = "foo4")
        {
            collection.Find(x=>x.Name == value, new FindOptions
            {
                CursorType = CursorType.Tailable
            })
        }

        public async void QueryUnindexedFieldCreatingIndex(string value = 22)
        {

        }

        public async void DeleteIndexOfFormallyUnindexedField()
        {

        }

        

        //public void 
    }
}
