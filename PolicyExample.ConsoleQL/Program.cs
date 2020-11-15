using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.SystemTextJson;

public class Program
{
    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Query
    {
        [GraphQLMetadata("hero")]
        public Droid GetHero()
        {
            return new Droid {Id = "1", Name = "R2-D2"};
        }
    }

    public static async Task Main(string[] args)
    {

        var schema = Schema.For(@"
      type Droid {
        id: String!
        name: String!
      }

      type Query {
        hero: Droid
      }
    ", _ => { _.Types.Include<Query>(); });

        var json = await schema.ExecuteAsync(_ => { _.Query = "{ hero { id name } }"; });
        
        Console.WriteLine(json);
    }
}