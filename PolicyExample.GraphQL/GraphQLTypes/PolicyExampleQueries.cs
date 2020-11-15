using System.Collections.Generic;
using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public sealed class PolicyExampleQueries : ObjectGraphType
    {
        public PolicyExampleQueries()
        {
            Field<ListGraphType<LanguageGraphType>>("language", resolve:r =>new[]{Language.JavaScript});
            Field<ListGraphType<ScriptServiceGraphType>>("scriptEngine", resolve: r => new[]{new ScriptEngine()
            {
                Id = "js",
                Name = "Jint",
                SupportedScriptLanguages = new List<Language>() {Language.JavaScript},
                Version = "0.1"
            }});
            
            Field<ListGraphType<ScriptServiceGraphType>>("scriptService", resolve: r => new []{new ScriptService()
            {
                Id = "logic-graph-flow",
                Name = "Logic Graph Flow",
                Version = "0.1"
            }});
        }
    }
}