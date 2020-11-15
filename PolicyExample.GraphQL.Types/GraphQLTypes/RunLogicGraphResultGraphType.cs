using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class RunLogicGraphResultGraphType : ObjectGraphType<RunLogicGraphResult>
    {
        public RunLogicGraphResultGraphType()
        {
            Field(c => c.Errors);
            Field(c => c.Success);
            Field(c => c.RunReport);
            Interface<CommandExecutionResultGraphType>();
        }
    }
}