using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PolicyExample.GraphQL.Types.DTO.Commands
{
    public class CreateScriptParams {
        public List<string> RequiredContextsIds { get; set; }
        public string Body { get; set; } 
        public string Language { get; set; }
    }
}