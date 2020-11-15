using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO
{
    public class LogicGraph 
    {
        public string Id { get; set; }
        public List<ScriptService> AvailableServices { get; set; }
        public string Name { get; set; }
        public int? Version { get; set; }
        public List<ScriptEngine> ProvidedEngines { get; set; }
        public List<LogicNode> Nodes { get; set; }
        public LogicNode Root { get; set; }
        public List<RunReport> RunHistory { get; set; }
    }
}