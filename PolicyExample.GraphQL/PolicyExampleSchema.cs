using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using PolicyExample.GraphQL.DTO;

namespace PolicyExample.GraphQL.GraphQLTypes
{
    public class PolicyExampleSchema : Schema
    {
        public PolicyExampleSchema(IServiceProvider resolver)
            : base(resolver)
        {
            Query = resolver.GetService<PolicyExampleQueries>();
            Mutation = resolver.GetService<PolicyExampleMutations>();
            
            RegisterType<LanguageGraphType>();
            RegisterType<ScriptEngineGraphType>();
            RegisterType<ScriptServiceGraphType>();
            RegisterType<ScriptServiceSchemaGraphType>();
            RegisterType<ScriptGraphType>();
            RegisterType<LogicNodeGraphType>();
            RegisterType<LogicGraphGraphType>();
            RegisterType<CommandExecutionResultGraphType>();
            RegisterType<CreateLogicGraphResultGraphType>();
            RegisterType<CreateLogicGraphCommandGraphType>();
        }
    }
}