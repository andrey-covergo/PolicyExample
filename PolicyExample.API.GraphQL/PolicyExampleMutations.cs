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
using PolicyExample.Scripting.Jint;
using ExecutionError = PolicyExample.Scripting.ExecutionError;
using LogicGraph = PolicyExample.Scripting.GraphLogic.LogicGraph;
using LogicNode = PolicyExample.Scripting.GraphLogic.LogicNode;
using NodeExecutionResult = PolicyExample.GraphQL.Types.DTO.NodeExecutionResult;
using Script = PolicyExample.Scripting.Jint.Script;

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
                            ExecutionFlow = new OrderedExecutionFlow(new JintNodeExecutor())
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

                    try
                    {
                        var graph = persistence.Graphs.First(g => g.Id == command.LogicGraphId);

                        var results = await graph.Graph.Run().ToListAsync();

                        return new RunLogicGraphResult()
                        {
                            Success = true, RunReport = new RunReport()
                            {
                                Id = command.Id,
                                Trace = results.Select(ToNodeExecutionResult).ToList()
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        return new RunLogicGraphResult()
                        {
                            Errors = new List<string>()
                            {
                                "Cannot run graph " + command.LogicGraphId,
                                ex.ToString()
                            },
                            Success = false
                        };
                    }
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

                        var logicGraphDomainObject = new LogicNode()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = command.Name,
                        };
                        if (command.Script?.Body != null)
                            logicGraphDomainObject.Script = new Script(command.Script?.Body);

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
                    return new NodeExecutionResult()
                    {
                        Errors = new List<string>() {executionError.Message}, 
                        Node =
                            new PolicyExample.GraphQL.Types.DTO.LogicNode()
                            {
                                Id=nodeVisit.Node.Id,
                                Name = nodeVisit.Node.Name
                            },
                    };
                }
                case ExecutionSuccess executionSuccess:
                case ExecutionSuccessAndContinue executionSuccessAndContinue:
                case ExecutionSuccessAndRedirect executionSuccessAndRedirect:
                case ExecutionSuccessAndStop executionSuccessAndStop:
                    return new NodeExecutionResult()
                    {
                        Errors = new List<string>() {nodeVisit.Result.Message}, 
                        Node =
                            new PolicyExample.GraphQL.Types.DTO.LogicNode()
                            {
                                Id=nodeVisit.Node.Id,
                                Name = nodeVisit.Node.Name
                            },
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}