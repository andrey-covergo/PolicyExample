using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Components.Forms;
using PolicyExample.GraphQL.Types.DTO;
using PolicyExample.GraphQL.Types.DTO.Commands;
using PolicyExample.GraphQL.Types.GraphQLTypes;
using PolicyExample.Scripting.GraphLogic;
using ExecutionError = PolicyExample.Scripting.ExecutionError;
using LogicGraph = PolicyExample.Scripting.GraphLogic.LogicGraph;
using NodeExecutionResult = PolicyExample.GraphQL.Types.DTO.NodeExecutionResult;

namespace PolicyExample.API.GraphQL
{
    public class LogicGraphEntity
    {
        public LogicGraph Graph { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Persistence
    {
        public readonly List<LogicGraphEntity> Graphs = new List<LogicGraphEntity>();
        
        
    }
    public sealed class PolicyExampleMutations : ObjectGraphType
    {


       
        
        public PolicyExampleMutations(Persistence persistence)
        {
            Field<CommandExecutionResultGraphType>()
                .Name("createNewLogicGraph")
                .Argument<CreateLogicGraphCommandGraphType>("command")
                .Resolve(ctx =>
                {
                    var command = ctx.GetArgument<CreateLogicGraphCommand>("command");

                    var logicGraphDomainObject = new LogicGraphEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = command.Name,
                        Graph = new LogicGraph()
                        {
                            ExecutionFlow = new JintOrderedExecutionFlow()
                        }
                    };
                    
                    persistence.Graphs.Add(logicGraphDomainObject);
                    
                    return new CreateLogicGraphResult() { Success = true, LogicGraphId = logicGraphDomainObject.Id};
                });
            
            Field<RunLogicGraphResultGraphType>()
                .Name("runLogicGraph")
                .Argument<RunLogicGraphCommandGraphType>("command")
                .ResolveAsync(async ctx =>
                {
                    var command = ctx.GetArgument<RunLogicGraphCommand>("command");
                    var graph = persistence.Graphs.First(g => g.Id == command.LogicGraphId);


                    var results = new List<NodeExecutionResult>();
                    await foreach (var nodeVisit in graph.Graph.Run())
                    {
                        results.Add(ToNodeExecutionResult(nodeVisit));
                    }

                    return new RunLogicGraphResult() { Success = true, RunReport = new RunReport()
                    {
                        Id = command.Id,
                        Trace = results
                    }};
                });
            
            Field<CommandExecutionResultGraphType>()
                .Name("createNewLogicNode")
                .Argument<CreateLogicNodeCommandGraphType>("command")
                .Resolve(ctx =>
                {
                    try
                    {
                        var command = ctx.GetArgument<CreateLogicNodeCommand>("command");
                        var graph = persistence.Graphs.First(g => g.Id == command.LogicGraphId);

                        var logicGraphDomainObject = new JintLogicNode()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = command.Name,
                            JavaScript = command.Script?.Body,

                        };

                        if (graph.Graph.Root == null)
                        {
                            graph.Graph.Root = logicGraphDomainObject;
                            return new CreateLogicNodeResult()
                                {Success = true, LogicNodeId = logicGraphDomainObject.Id};
                        }

                        var parentNode = graph.Graph.Root.GetAllChildrenNodes()
                            .FirstOrDefault(n => n.Id == command.ParentNodeId);

                        if (parentNode == null)
                        {
                            return new CreateLogicNodeResult()
                            {
                                Errors = new List<string>() {"Cannot find parent node with id " + command.ParentNodeId},
                                Success = false
                            };
                        }

                        logicGraphDomainObject.Parent = parentNode;
                        parentNode.Children.Add(logicGraphDomainObject);

                        return new CreateLogicNodeResult() {Success = true, LogicNodeId = logicGraphDomainObject.Id};
                    }
                    catch(Exception ex)
                    {
                        return new CreateLogicNodeResult() {Success = false, Errors = new List<string>(){ex.ToString()}};
                    }
                });
        }

        private static NodeExecutionResult ToNodeExecutionResult(NodeVisitResult nodeVisit)
        {
            switch (nodeVisit.Result)
            {
                case null:
                    return null;
                case ExecutionError executionError:
                {
                   return new NodeExecutionError()
                    {
                        Error = executionError.Message, NodeID = nodeVisit.Node.Id
                    };
                }
                case ExecutionSuccess executionSuccess:
                case ExecutionSuccessAndContinue executionSuccessAndContinue:
                case ExecutionSuccessAndRedirect executionSuccessAndRedirect:
                case ExecutionSuccessAndStop executionSuccessAndStop:
                    return new NodeExecutionSuccess() {Content = nodeVisit.Result.Message, NodeID = nodeVisit.Node.Id};
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}