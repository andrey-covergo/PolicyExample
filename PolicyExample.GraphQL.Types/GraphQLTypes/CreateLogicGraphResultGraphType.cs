using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public class CreateLogicGraphResultGraphType :  ObjectGraphType<CreateLogicGraphResult>
    {
        public CreateLogicGraphResultGraphType()
        {
            Field(d => d.Success);
            Field(d => d.Errors, type:typeof(ListGraphType<StringGraphType>));
            Field(d => d.LogicGraphId);
            Interface<CommandExecutionResultGraphType>();
        }
    }
}