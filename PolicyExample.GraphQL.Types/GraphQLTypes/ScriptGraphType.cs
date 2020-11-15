using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public class ScriptGraphType:ObjectGraphType<Script> 
    {
        public ScriptGraphType()
        {
            Field(g => g.Id);
            Field(g => g.Body);
            Field(g => g.Language).Type(new LanguageGraphType());
            Field(g => g.RequiredServices, type: typeof(ListGraphType<ScriptServiceGraphType>));
        }
    }
}