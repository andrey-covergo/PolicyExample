using System.Threading.Tasks;
using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    
    public sealed class ScriptServiceGraphType:ObjectGraphType<ScriptService>{
        public ScriptServiceGraphType()
        {
            Field(s => s.Id);
            Field(s => s.Name);
            Field(s => s.Version);
        }
    }
}