using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class CreateLogicGraphCommandGraphType : InputObjectGraphType<CreateLogicGraphCommand>
    {
        public CreateLogicGraphCommandGraphType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
            Field(c => c.ProvidedContexts, type:typeof(ListGraphType<StringGraphType>));
            Field(c => c.ProvidedEngines, type: typeof(ListGraphType<StringGraphType>));
        }
    }
    
    

        public sealed class CreateScriptParamsGraphType : InputObjectGraphType<CreateScriptParams>
        {
            public CreateScriptParamsGraphType()
            {
                Field(s => s.Body);
            }
        }


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