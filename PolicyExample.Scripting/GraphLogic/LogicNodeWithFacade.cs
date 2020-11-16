using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public class LogicNodeWithFacade : LogicNode
    {
         public readonly NodeFlowService Facade;
        public LogicNodeWithFacade()
        {   
          Facade = new NodeFlowService(this);
        }
    }
}