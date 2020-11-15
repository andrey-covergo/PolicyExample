using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using GraphQL;

namespace PolicyExample.GraphQL.Types {
  public class PolicyExampleGraphQLTypes {
    
    #region LogicNode
    /// <summary>
    /// A node in a logic tree. If ParentID is empty, than it consider as root.
    /// </summary>
    public class LogicNode {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("Name")]
      public string Name { get; set; }
    
      [JsonProperty("Script")]
      public Script Script { get; set; }
    
      [JsonProperty("ParentId")]
      public LogicNode ParentId { get; set; }
    
      [JsonProperty("Children")]
      public List<LogicNode> Children { get; set; }
      #endregion
    }
    #endregion
    
    #region LogicGraph
    public class LogicGraph {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("ProvidedContexts")]
      public List<ScriptContext> ProvidedContexts { get; set; }
    
      [JsonProperty("Name")]
      public string Name { get; set; }
    
      [JsonProperty("Version")]
      public int? Version { get; set; }
    
      [JsonProperty("ProvidedEngines")]
      public List<ScriptEngine> ProvidedEngines { get; set; }
    
      [JsonProperty("Nodes")]
      public List<LogicNode> Nodes { get; set; }
    
      [JsonProperty("Root")]
      public LogicNode Root { get; set; }
    
      [JsonProperty("RunHistory")]
      public List<RunReport> RunHistory { get; set; }
      #endregion
    }
    #endregion
    
    #region ScriptContext
    public class ScriptContext {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("Name")]
      public string Name { get; set; }
    
      [JsonProperty("Version")]
      public int? Version { get; set; }
      #endregion
    }
    #endregion
    
    #region ScriptEngine
    /// <summary>
    /// Defines scripting language and execution runtime. Now supporting only Jint for JavaScript (ECMA 5.1)
    /// </summary>
    public class ScriptEngine {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("Name")]
      public string Name { get; set; }
    
      [JsonProperty("Version")]
      public int? Version { get; set; }
    
      [JsonProperty("SupportedScriptLanguages")]
      public List<Language> SupportedScriptLanguages { get; set; }
      #endregion
    }
    #endregion
    
    #region ScriptContextSchema
    /// <summary>
    /// Describes a context available for a script during its execution. Context Schema representation depends on the Engine as well. Schema must be comprehensive for concrete Script language. For Jint engine context schema is a JSON schema
    /// </summary>
    public class ScriptContextSchema {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("ScriptContext")]
      public ScriptContext ScriptContext { get; set; }
    
      [JsonProperty("Schema")]
      public string Schema { get; set; }
    
      [JsonProperty("Engine")]
      public ScriptEngine Engine { get; set; }
      #endregion
    }
    #endregion
    
    #region Script
    public class Script {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("RequiredContexts")]
      public List<ScriptContext> RequiredContexts { get; set; }
    
      [JsonProperty("Body")]
      public string Body { get; set; }
    
      [JsonProperty("Language")]
      public string Language { get; set; }
      #endregion
    }
    #endregion
    
    #region RunReport
    public class RunReport {
      #region members
      [JsonProperty("Id")]
      public string Id { get; set; }
    
      [JsonProperty("ScriptEngine")]
      public ScriptEngine ScriptEngine { get; set; }
    
      [JsonProperty("Trace")]
      public List<NodeExecutionResult> Trace { get; set; }
    
      [JsonProperty("Time")]
      public DateTimeOffset Time { get; set; }
      #endregion
    }
    #endregion
    
    public interface NodeExecutionResult {
      [JsonProperty("NodeID")]
      public string NodeID { get; set; }
    
      [JsonProperty("Time")]
      public DateTimeOffset Time { get; set; }
    
      [JsonProperty("Output")]
      public string Output { get; set; }
    }
    
    #region NodeExecutionSuccess
    public class NodeExecutionSuccess : NodeExecutionResult {
      #region members
      [JsonProperty("NodeID")]
      public string NodeID { get; set; }
    
      [JsonProperty("Content")]
      public string Content { get; set; }
    
      [JsonProperty("Time")]
      public DateTimeOffset Time { get; set; }
    
      [JsonProperty("Output")]
      public string Output { get; set; }
      #endregion
    }
    #endregion
    
    #region NodeExecutionError
    public class NodeExecutionError : NodeExecutionResult {
      #region members
      [JsonProperty("NodeID")]
      public string NodeID { get; set; }
    
      [JsonProperty("Error")]
      public string Error { get; set; }
    
      [JsonProperty("Time")]
      public DateTimeOffset Time { get; set; }
    
      [JsonProperty("Output")]
      public string Output { get; set; }
      #endregion
    }
    #endregion
    
    public interface CommandExecutionResult {
      [JsonProperty("Errors")]
      public List<string> Errors { get; set; }
    
      [JsonProperty("Success")]
      public bool Success { get; set; }
    }
    
    #region CreateLogicGraphResult
    public class CreateLogicGraphResult : CommandExecutionResult {
      #region members
      [JsonProperty("Errors")]
      public List<string> Errors { get; set; }
    
      [JsonProperty("Success")]
      public bool Success { get; set; }
    
      [JsonProperty("LogicGraphId")]
      public string LogicGraphId { get; set; }
      #endregion
    }
    #endregion
    
    #region CreateLogicNodeResult
    public class CreateLogicNodeResult : CommandExecutionResult {
      #region members
      [JsonProperty("Errors")]
      public List<string> Errors { get; set; }
    
      [JsonProperty("Success")]
      public bool Success { get; set; }
    
      [JsonProperty("LogicNodeId")]
      public string LogicNodeId { get; set; }
      #endregion
    }
    #endregion
    
    #region RunLogicGraphResult
    public class RunLogicGraphResult : CommandExecutionResult {
      #region members
      [JsonProperty("Errors")]
      public List<string> Errors { get; set; }
    
      [JsonProperty("Success")]
      public bool Success { get; set; }
    
      [JsonProperty("RunReport")]
      public RunReport RunReport { get; set; }
      #endregion
    }
    #endregion
    
    #region RunLogicGraphParams
    public class RunLogicGraphParams {
      #region members
      public string Id { get; set; }
    
      public string LogicGraphId { get; set; }
    
      public CreateScriptParams ConfigurationScript { get; set; }
    
      public List<string> RequiredContexts { get; set; }
      #endregion
    
      #region methods
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
      #endregion
    }
    #endregion
    
    #region CreateScriptParams
    public class CreateScriptParams {
      #region members
      public List<string> RequiredContextsIds { get; set; }
    
      [JsonRequired]
      public string Body { get; set; }
    
      [JsonRequired]
      public string Language { get; set; }
      #endregion
    
      #region methods
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
      #endregion
    }
    #endregion
    
    #region CreateLogicGraphParams
    public class CreateLogicGraphParams {
      #region members
      [JsonRequired]
      public string Id { get; set; }
    
      [JsonRequired]
      public string Name { get; set; }
    
      public List<CreateLogicNodeParams> Nodes { get; set; }
    
      public List<string> ProvidedContexts { get; set; }
    
      public List<string> ProvidedEngines { get; set; }
      #endregion
    
      #region methods
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
      #endregion
    }
    #endregion
    
    #region CreateLogicNodeParams
    public class CreateLogicNodeParams {
      #region members
      public string NodeId { get; set; }
    
      public string LogicGraphId { get; set; }
    
      [JsonRequired]
      public string Name { get; set; }
    
      public CreateScriptParams Script { get; set; }
    
      public string ParentNodeId { get; set; }
      #endregion
    
      #region methods
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
      #endregion
    }
    #endregion
    public enum Language {
      JavaScript
    }
    
    
    #region PolicyExampleQueries
    public class PolicyExampleQueries {
      #region members
      [JsonProperty("GetSupportedContexts")]
      public List<ScriptContext> GetSupportedContexts { get; set; }
    
      [JsonProperty("GetContextSchemas")]
      public List<ScriptContextSchema> GetContextSchemas { get; set; }
      #endregion
    }
    #endregion
    
    #region PolicyExampleCommands
    public class PolicyExampleCommands {
      #region members
      [JsonProperty("CreateLogicGraphCommand")]
      public CreateLogicGraphResult CreateLogicGraphCommand { get; set; }
    
      [JsonProperty("CreateLogicNodeCommand")]
      public CreateLogicNodeResult CreateLogicNodeCommand { get; set; }
    
      [JsonProperty("RunLogicGraphCommand")]
      public RunLogicGraphResult RunLogicGraphCommand { get; set; }
      #endregion
    }
    #endregion
  }
  
}
