using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODA.Mongo
{
    public class FullTextSearch
    {
        MongoClient mongoClient;
        private IMongoDatabase database;

        public FullTextSearch(string databaseName = "test", MongoUrl url = null)
        {
            mongoClient = url == null ? new MongoClient() : new MongoClient(url);
            database = mongoClient.GetDatabase(databaseName);
            
            database.CreateCollectionAsync("Students").Wait();
        }
    }
}
