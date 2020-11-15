using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public sealed class CommandExecutionResultGraphType : InterfaceGraphType<CommandExecutionResult>
    {
        public CommandExecutionResultGraphType()
        {
            Field(d => d.Success);
            Field(d => d.Errors, type:typeof(ListGraphType<StringGraphType>));
        }
    }
}