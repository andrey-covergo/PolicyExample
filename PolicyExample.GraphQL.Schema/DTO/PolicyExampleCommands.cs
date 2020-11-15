using PolicyExample.GraphQL.Schema.DTO.Commands;

namespace PolicyExample.GraphQL.Schema.DTO
{
    public class PolicyExampleCommands {
     
        
        public CreateLogicGraphResult CreateLogicGraphCommand { get; set; }
    
        
        public CreateLogicNodeResult CreateLogicNodeCommand { get; set; }
    
        
        public RunLogicGraphResult RunLogicGraphCommand { get; set; }
        
    }
}