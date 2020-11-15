using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class RunLogicGraphCommandGraphType : InputObjectGraphType<RunLogicGraphCommand>
    {
        public RunLogicGraphCommandGraphType()
        {
            Field(c => c.Id);
            Field(c => c.LogicGraphId);
            Field(c => c.ConfigurationScript, type: typeof(CreateScriptParamsGraphType));
            Field(c => c.RequiredContexts, type:typeof(ListGraphType<StringGraphType>));
        }
    }

    public sealed class NodeVisitResultGraphType : ObjectGraphType<NodeExecutionResult>
    {
        public NodeVisitResultGraphType()
        {
            Field(f => f.Node,type:typeof(LogicNodeGraphType));
            Field(f => f.Output);
            Field(f => f.Errors,type:typeof(ListGraphType<StringGraphType>));
        }
    }

     

    
    public sealed class  RunReportGraphType : ObjectGraphType<RunReport> {
        public RunReportGraphType()
        {
            Field(r => r.Id);
            Field(r => r.ScriptEngine, type:typeof(ScriptEngineGraphType));
            Field(r => r.Trace, type:typeof(ListGraphType<NodeVisitResultGraphType>));
        }
    }
}