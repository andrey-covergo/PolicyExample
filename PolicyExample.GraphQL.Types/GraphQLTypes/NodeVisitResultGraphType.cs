using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class NodeVisitResultGraphType : ObjectGraphType<NodeExecutionResult>
    {
        public NodeVisitResultGraphType()
        {
            Field(f => f.Node,type:typeof(LogicNodeGraphType));
            Field(f => f.Output);
            Field(f => f.Errors,type:typeof(ListGraphType<StringGraphType>));
        }
    }
}