using GraphQL.Types;
using PolicyExample.GraphQL.Schema.DTO;

namespace PolicyExample.GraphQL.Schema.GraphQLTypes
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