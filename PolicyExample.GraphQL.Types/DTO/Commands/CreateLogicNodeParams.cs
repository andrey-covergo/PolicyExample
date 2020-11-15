using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PolicyExample.GraphQL.Types.DTO.Commands
{
    public class CreateLogicNodeCommand {
     
        public string Id { get; set; }
    
        public string LogicGraphId { get; set; }
    
        [JsonRequired]
        public string Name { get; set; }
    
        public CreateScriptParams Script { get; set; }
    
        public string? ParentNodeId { get; set; }
        
    
     
        public dynamic GetInputObject()
        {
            IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
            var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(this);
                var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
                var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
                if (requiredProp || value != defaultValue)
                {
                    d[propertyInfo.Name] = value;
                }
            }
            return d;
        }
        
    }
}