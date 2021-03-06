using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class CreateLogicNodeResultGraphType :  ObjectGraphType<CreateLogicNodeResult>
    {
        public CreateLogicNodeResultGraphType()
        {
            Field(d => d.Success);
            Field(d => d.Errors, type:typeof(ListGraphType<StringGraphType>));
            Field(d => d.LogicNodeId, nullable:true);
            Interface<CommandExecutionResultGraphType>();
        }
    }
}