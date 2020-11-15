using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class ScriptEngineGraphType : ObjectGraphType<ScriptEngine>
    {
        public ScriptEngineGraphType()
        {
            Field(d => d.Id);
            Field(d => d.Name);
            Field(d => d.Version);
            Field(d => d.SupportedScriptLanguages,type: typeof(ListGraphType<LanguageGraphType>));
        }
    }
}