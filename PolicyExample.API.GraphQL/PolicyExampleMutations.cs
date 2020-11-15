using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;
using PolicyExample.GraphQL.Types.GraphQLTypes;
using PolicyExample.Scripting.GraphLogic;

namespace PolicyExample.API.GraphQL
{
    public sealed class PolicyExampleMutations : ObjectGraphType
    {

        private readonly List<LogicGraphEntity> _graphs = new List<LogicGraphEntity>();

        class LogicGraphEntity
        {
            public LogicGraph Graph { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
        }
        
        public PolicyExampleMutations()
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
                    
                    _graphs.Add(logicGraphDomainObject);
                    
                    return new CreateLogicGraphResult() { Success = true, LogicGraphId = logicGraphDomainObject.Id};
                });
        }

    }
}