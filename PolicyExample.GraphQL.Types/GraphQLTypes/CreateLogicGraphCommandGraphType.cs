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
}