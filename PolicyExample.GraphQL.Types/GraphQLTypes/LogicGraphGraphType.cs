using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class LogicGraphGraphType : ObjectGraphType<LogicGraph>
    {
        public LogicGraphGraphType()
        {
            Field(d => d.Id);
            Field(d => d.Name);
            Field(d => d.Version, type:typeof(IntGraphType), nullable:true);
            Field(d => d.AvailableServices, type:typeof(ListGraphType<ScriptServiceGraphType>), nullable:true);
            Field(d => d.ProvidedEngines, type:typeof(ListGraphType<ScriptEngineGraphType>), nullable:true);
            Field(d => d.Nodes, type:typeof(ListGraphType<LogicNodeGraphType>), nullable:true);
            Field(d => d.Root, type:typeof(LogicNodeGraphType), nullable:true);
            //Field(d => d.RunHistory, type:typeof(LogicNodeGraphType), nullable:true);
        }
    }
}