using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TODA
{
    public class Test
    {

        public class User
        {
            public string Name { get; set; }
            public string Id { get; set; }
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
        public void RunInsert()
        {
            var client = new MongoClient(); ;
            var database = client.GetDatabase("test");
            try {
                database.CreateCollectionAsync("Users").Wait();
                var usersCollection = database.GetCollection<User>("Users", new MongoCollectionSettings
                {
                    AssignIdOnInsert=true                    
                });
                usersCollection.InsertManyAsync(Enumerable.Range(0, 1000).Select(x => new User
                {
                    Name="foo"+x,
                    Age=x%40,
                    Address=new Address
                    {
                        Appartment=x%15,
                        City="City"+x%4,
                        Country="Country"+x%3,
                        House=x%200
                    }
                }));
            }
            finally
            {
             
            }
        }


        public void CreateIndexes()
        {
            var client = new MongoClient(); ;
            var database = client.GetDatabase("test");

            var collection = database.GetCollection<User>("Users");
            var indexKeysDefinition = new IndexKeysDefinitionBuilder<User>();
            Expression<Func<User, String>> userNameExpression = x=>x.Name;            
                        
            var field = new ExpressionFieldDefinition<User>(userNameExpression);
            indexKeysDefinition.Ascending()
            collection.Indexes.CreateOneAsync(new IndexKeysDefinition<User>())
        }
    }
}
