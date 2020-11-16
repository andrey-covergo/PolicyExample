using System;
using System.Threading.Tasks;
using PolicyExample.GraphQL.Client;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.Tests.Integration
{
    public static class GraphQLCientExtensions
    {
        public static async Task<CreateLogicNodeResult> CreateNode(this PolicyExampleGraphQLClient client, string graphId,
            string nodeName, string? parentId=null, string? script=null)
        {
            CreateScriptParams? scriptParams=null;

            if (script != null)
            {
                scriptParams = new CreateScriptParams()
                {
                    Body = script,
                    Language = "JavaScript"
                };
            }
            
            return await client.Execute(new CreateLogicNodeCommand()
            {
                Id = Guid.NewGuid().ToString(),
                LogicGraphId = graphId,
                Script = scriptParams,
                ParentNodeId = parentId,
                Name = nodeName
            });
        }
    }
}