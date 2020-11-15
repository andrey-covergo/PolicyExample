using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public class PolicyExampleSchema : Schema
    {
        public PolicyExampleSchema(IServiceProvider resolver)
            : base(resolver)
        {
            Query = resolver.GetService<PolicyExampleQuery>();
        }
    }
}