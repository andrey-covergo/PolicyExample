using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
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