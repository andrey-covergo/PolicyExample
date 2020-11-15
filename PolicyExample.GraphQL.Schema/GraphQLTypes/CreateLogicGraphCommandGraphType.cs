using GraphQL.Types;
using PolicyExample.GraphQL.Schema.DTO.Commands;

namespace PolicyExample.GraphQL.Schema.GraphQLTypes
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
}