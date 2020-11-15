using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public class LogicGraphType : ObjectGraphType<LogicGraph>
    {
        public LogicGraphType()
        {
            Field(d => d.Id);
            Field(d => d.ProvidedContexts);
            Field(d => d.Name);
            Field(d => d.Version);
            Field(d => d.ProvidedEngines);
            Field(d => d.Nodes);
            Field(d => d.Root);
            Field(d => d.RunHistory);
        }
    }
}