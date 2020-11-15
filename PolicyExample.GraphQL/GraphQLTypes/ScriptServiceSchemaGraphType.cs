using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public sealed class ScriptServiceSchemaGraphType:ObjectGraphType<ScriptServiceSchema>{
        public ScriptServiceSchemaGraphType()
        {
            Field(s => s.Id);
            Field(s => s.Schema);
            Field(s => s.Engine, type: typeof(ScriptEngineGraphType));
            Field(s => s.Service, type: typeof(ScriptServiceGraphType));
        }
    }
}