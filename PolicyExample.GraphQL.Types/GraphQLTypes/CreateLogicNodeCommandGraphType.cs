using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class CreateLogicNodeCommandGraphType : InputObjectGraphType<CreateLogicNodeCommand>
    {
        public CreateLogicNodeCommandGraphType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
            Field(c => c.Script, type:typeof(CreateScriptParamsGraphType));
            Field(c => c.LogicGraphId);
            Field(c => c.ParentNodeId, nullable:true);
        }
    }
}