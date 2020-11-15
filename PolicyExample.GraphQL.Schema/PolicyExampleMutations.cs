using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using PolicyExample.GraphQL.Schema.DTO.Commands;
using PolicyExample.GraphQL.Schema.GraphQLTypes;

namespace PolicyExample.GraphQL.Schema
{
    public sealed class PolicyExampleMutations : ObjectGraphType
    {
        public PolicyExampleMutations()
        {
            Field<CommandExecutionResultGraphType>()
                .Name("createNewLogicGraph")
                .Argument<CreateLogicGraphCommandGraphType>("command")
                .Resolve(ctx =>
                {
                    var command = ctx.GetArgument<CreateLogicGraphCommand>("command");
                    var error = new NotImplementedException();
                    return new CreateLogicGraphResult() { Success = false, Errors  = new List<string>(){error.ToString()}};
                });
        }

    }
}