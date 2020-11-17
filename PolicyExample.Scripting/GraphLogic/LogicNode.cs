using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExample.Scripting.Jint;

namespace PolicyExample.Scripting.GraphLogic
{
    //TODO: find a proper graph library
    public class LogicNode
    {
        public LogicNode Parent { get; set; }
        public List<LogicNode> Children { get;  } = new List<LogicNode>();
        public string Name { get; set; }
        public string Id { get; set; }

        public IScript? Script { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}