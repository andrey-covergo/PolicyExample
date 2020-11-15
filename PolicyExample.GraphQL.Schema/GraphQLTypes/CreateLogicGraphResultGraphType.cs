using GraphQL.Types;
using PolicyExample.GraphQL.Schema.DTO.Commands;

namespace PolicyExample.GraphQL.Schema.GraphQLTypes
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