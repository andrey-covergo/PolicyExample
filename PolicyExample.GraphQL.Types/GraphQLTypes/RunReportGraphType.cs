using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class  RunReportGraphType : ObjectGraphType<RunReport> {
        public RunReportGraphType()
        {
            Field(r => r.Id);
            Field(r => r.ScriptEngine, type:typeof(ScriptEngineGraphType));
            Field(r => r.Trace, type:typeof(ListGraphType<NodeVisitResultGraphType>));
        }
    }
}