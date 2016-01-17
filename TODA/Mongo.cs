using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using MongoDB.Driver.Core.Clusters.ServerSelectors;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TODA.Model;

namespace TODA
{
    public class Mongo2
    {      
        public void InsertStudents()
        {
            var client = new MongoClient(); ;
            var database = client.GetDatabase("test");
            try {
                database.CreateCollectionAsync("Users").Wait();
                var usersCollection = database.GetCollection<Student>("Users", new MongoCollectionSettings
                {
                    AssignIdOnInsert=true                    
                });
                usersCollection.InsertManyAsync(Enumerable.Range(0, 1000).Select(x => new Student
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
        public void Subscription() { }
        public void Authentication() { }
        public void Replication() { }
        public void Sharding() { }      
        
        
        public void InserClassesWFullStudents()
        {
            
            var client = new MongoClient(new MongoClientSettings
            {
                ClusterConfigurator = clusterBuilder =>
                {
                    clusterBuilder.ConfigureConnection(connection =>
                    {
                        connection.With(new IAuthenticator[] {
                            new MongoDBCRAuthenticator(new UsernamePasswordCredential(string.Empty, "foo", "bar"))                           
                        }, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(100));                     
                        return connection;

                    });

                  
                }
            });

        }

        


        public void CreateIndexes()
        {
            var client = new MongoClient(); ;
            var database = client.GetDatabase("test");

            var collection = database.GetCollection<Student>("Users");
            var indexKeysDefinition = new IndexKeysDefinitionBuilder<Student>();
            Expression<Func<Student, String>> userNameExpression = x=>x.Name;            
                        
            var field = new ExpressionFieldDefinition<Student>(userNameExpression);
            //indexKeysDefinition.Ascending();   
            //collection.Indexes.CreateOneAsync(new IndexKeysDefinition<Student>())
        }
    }
}
