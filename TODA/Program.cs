using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODA.Mongo;

namespace TODA
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Mongo().RunInsert();
            var mongoUrl = new MongoUrlBuilder();
            mongoUrl.Server = new MongoServerAddress("localhost", 27071);

            MongoUrl mongoUrl1 = null;
            var crudTest = new Crud("test", mongoUrl1);
            crudTest.Init().Wait();
            crudTest.Create(1000).Wait();
            crudTest.Read(1000).Wait();
            //crudTest.OneStepDelete(1000).Wait();

        }
    }
}
