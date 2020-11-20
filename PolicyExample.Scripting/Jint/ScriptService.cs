using System;
using System.Data;
using SlugityLib;

namespace PolicyExample.Scripting.Jint
{

    public class KnownServices
    {
        public static ScriptService ExecutionFlowService = new ScriptService() {Name = "Execution flow service"};
        public static ScriptServiceSchema ExecutionFlowServiceSchema =  new ScriptServiceSchema() {AccessName = "flow"};
    }

    public class ScriptServiceSchema
    {
        public string AccessName { get; set; }
        public string Description { get; set; }
        public Language? Language { get; set; }
        public string Version { get; set; }
        private static Slugity _slugity= new Slugity();
        public static ScriptServiceSchema Default(ScriptService service)
        {
            return new ScriptServiceSchema() {AccessName = _slugity.GenerateSlug(service.Name)};
        }
    }
    
    public class ScriptService : IEquatable<ScriptService>
    {
        public bool Equals(ScriptService? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Version == other.Version;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScriptService) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Version);
        }

        public static bool operator ==(ScriptService? left, ScriptService? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScriptService? left, ScriptService? right)
        {
            return !Equals(left, right);
        }

        public string Name { get; set; }
        public int Version { get; set; }
    }
}