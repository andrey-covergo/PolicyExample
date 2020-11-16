using System;
using System.Threading.Tasks;
using Jint;

namespace PolicyExample.Scripting.GraphLogic
{
    public class JintLogicNode : LogicNodeWithFacade
    {
        public string? JavaScript { get; set; }
    }
}