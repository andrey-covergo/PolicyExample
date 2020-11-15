using System;
using Microsoft.Extensions.DependencyInjection;
using PolicyExample.GraphQL.Schema.GraphQLTypes;

namespace PolicyExample.GraphQL.Schema
{
    public class PolicyExampleSchema : global::GraphQL.Types.Schema
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