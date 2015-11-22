using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODA.Mongo
{
    public class Querying
    {
        public void CreateIndexes()
        {
            var client = new MongoClient(); ;
            var database = client.GetDatabase("test");

            var collection = database.GetCollection<Student>("Users");
            var indexKeysDefinition = new IndexKeysDefinitionBuilder<Student>();
            Expression<Func<Student, String>> userNameExpression = x => x.Name;

            var field = new ExpressionFieldDefinition<Student>(userNameExpression);
            indexKeysDefinition.Ascending()
            collection.Indexes.CreateOneAsync(new IndexKeysDefinition<Student>())
        }
    }
}
