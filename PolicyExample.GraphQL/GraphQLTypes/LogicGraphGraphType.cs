using GraphQL.Types;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public sealed class LogicGraphGraphType : ObjectGraphType<LogicGraph>
    {
        public LogicGraphGraphType()
        {
            Field(d => d.Id);
            Field(d => d.Name);
            Field(d => d.Version, type:typeof(IntGraphType), nullable:true);
        }
    }
}