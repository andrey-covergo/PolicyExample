using GraphQL.Types;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.GraphQLTypes
{
    public sealed class CreateScriptParamsGraphType : InputObjectGraphType<CreateScriptParams>
    {
        public CreateScriptParamsGraphType()
        {
            Field(s => s.Body);
            Field(s => s.Language);
            Field(s => s.RequiredContextsIds,type:typeof(ListGraphType<StringGraphType>));
        }
    }
}