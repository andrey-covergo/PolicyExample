using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;
using PolicyExample.GraphQL.Types.GraphQLTypes;
using PolicyExample.Scripting.GraphLogic;
using LogicGraph = PolicyExample.GraphQL.Types.DTO.LogicGraph;
using LogicNode = PolicyExample.GraphQL.Types.DTO.LogicNode;

namespace PolicyExample.API.GraphQL
{
    
   
    public sealed class PolicyExampleQueries : ObjectGraphType
    {
        public PolicyExampleQueries(Persistence persistence)
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
    var flow = {
        stop : function(){} // returns nothing
        redirectToParent : function(){} // returns nothing
        redirectToChild: function(childNum){} // returns bool 
    }"
            };
            
            var templateGraph = new LogicGraph()
            {
                Id ="1",
                Name="graph template",
                AvailableServices = new List<ScriptService>(){logicGraphFlowService},
                AvailableEngines = new List<ScriptEngine>(){jintScriptEngine}
            };
            
            var goToSecondChildScript = new Script()
            {
                Body="flow.redirectToChild(1);",
                Id="redirectToChild1",
                Language = Language.JavaScript,
                RequiredServices = new List<ScriptService>(){logicGraphFlowService}
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
                .Name("scriptServiceSchema")
                .Resolve( r =>
            {
              
                return new[]
                {
                    nodeFlowServiceSchema
                };
            });
            
            Field<ListGraphType<ScriptGraphType>>()
                .Name("script")
                .Resolve( r =>
                {
                    return new[]
                    {
                        goToSecondChildScript
                    };
                });

            Field<ListGraphType<LogicNodeGraphType>>()
                .Name("logicNode")
                .Resolve(r =>
                {
                    return new[]
                    {
                        new LogicNode()
                        {
                           Id="1",
                           Name="Root",
                           Script = goToSecondChildScript 
                        }
                    };
                });
            
            Field<ListGraphType<LogicGraphGraphType>>()
                .Name("logicGraph")
                .Resolve(r =>
                {
                    return persistence.Graphs.Select(p => new LogicGraph()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        AvailableServices = new List<ScriptService>() {logicGraphFlowService},
                        AvailableEngines = new List<ScriptEngine>() {jintScriptEngine}
                    }).ToArray();
                });
        }
    }

    public static class LogicNodeExtensinos
    {
        // public static LogicGraph ToDto(this PolicyExample.Scripting.GraphLogic.LogicGraph p)
        // {
        //     return new LogicGraph()
        //     {
        //         Id = p.Id,
        //         Name = p.Name,
        //         AvailableServices = new List<ScriptService>(){logicGraphFlowService},
        //         Nodes = NodeExtensions.GetAllChildrenNodes(p.Graph.Root)
        //             .Select(node => node.ToDto())
        //                                             
        //     }
        // }
        //
        // public static LogicNode ToDto(this PolicyExample.Scripting.GraphLogic.LogicNode node)
        // {
        //     var node = new LogicNode()
        //     {
        //         
        //     }
        // }
    }
    
}