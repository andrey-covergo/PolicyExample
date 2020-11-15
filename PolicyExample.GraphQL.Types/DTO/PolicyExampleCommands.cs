using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Types.DTO
{
    public class PolicyExampleCommands {
     
        
        public CreateLogicGraphResult CreateLogicGraphCommand { get; set; }
    
        
        public CreateLogicNodeResult CreateLogicNodeCommand { get; set; }
    
        
        public RunLogicGraphResult RunLogicGraphCommand { get; set; }
        
    }
}