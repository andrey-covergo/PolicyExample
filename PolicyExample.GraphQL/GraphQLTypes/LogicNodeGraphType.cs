using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public class LogicNodeGraphType:ObjectGraphType<LogicNode>  {

        public LogicNodeGraphType()
        {
            Field(n => n.Id);
            Field(n => n.Name);
            Field(n => n.Script, type: typeof(ScriptGraphType));
            Field(n => n.Parent, type: typeof(LogicNodeGraphType));
            Field(n => n.Children, type: typeof(ListGraphType<LogicNodeGraphType>));
        }
    }
}