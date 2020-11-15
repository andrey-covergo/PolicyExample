using GraphQL.Types;
using PolicyExample.GraphQL.Schema.DTO;

namespace PolicyExample.GraphQL.Schema.GraphQLTypes
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