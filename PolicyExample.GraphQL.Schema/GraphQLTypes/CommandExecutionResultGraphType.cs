using GraphQL.Types;
using PolicyExample.GraphQL.Schema.DTO.Commands;

namespace PolicyExample.GraphQL.Schema.GraphQLTypes
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