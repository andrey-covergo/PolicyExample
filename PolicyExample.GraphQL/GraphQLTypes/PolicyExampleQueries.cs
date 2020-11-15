using System.Collections.Generic;
using GraphQL.Types;
using NJsonSchema;
using PolicyExample.GraphQL.DTO;
using PolicyExample.Scripting.GraphLogic;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public sealed class PolicyExampleQueries : ObjectGraphType
    {
        public PolicyExampleQueries()
        {
            var jintScriptEngine = new ScriptEngine()
            {
                Id = "js",
                Name = "Jint",
                SupportedScriptLanguages = new List<Language>() {Language.JavaScript},
                Version = "0.1"
            };
            var logicGraphFlowService = new ScriptService()
            {
                Id = "logic-graph-flow",
                Name = "Logic Graph Flow",
                Version = "0.1"
            };
            var nodeFlowServiceSchema = new ScriptServiceSchema()
            {
                Id = "logic-graph-flow-json-schema",
                Service = logicGraphFlowService,
                Engine = jintScriptEngine,
                Schema = @"
    var INodeFlowService = {
        stop : function(){} // returns nothing
        redirectToParent : function(){} // returns nothing
        redirectToChild: function(childNum){} // returns bool 
    }"
            };
            
            
            Field<ListGraphType<LanguageGraphType>>("language", resolve:r =>new[]{Language.JavaScript});
            Field<ListGraphType<ScriptServiceGraphType>>("scriptEngine", resolve: r =>
            {
                return new[] {jintScriptEngine};
            });
            Field<ListGraphType<ScriptServiceGraphType>>()
                .Name("scriptService")
                .Resolve( resolve: r =>
            {
                return new[] {logicGraphFlowService};
            });
            
            Field<ListGraphType<ScriptServiceSchemaGraphType>>()
                .Name("scriptService")
                .Resolve( r =>
            {
              
                return new[]
                {
                    nodeFlowServiceSchema
                };
            });
        }
    }
}