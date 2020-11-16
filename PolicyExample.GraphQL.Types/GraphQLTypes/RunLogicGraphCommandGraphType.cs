using GraphQL.Types;
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
}