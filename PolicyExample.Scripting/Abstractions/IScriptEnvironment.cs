namespace PolicyExample.Scripting.Abstractions
{
    /// <summary>
    /// T expects a script will call any public method or property T exposes
    /// </summary> 
    /// <typeparam name="T"></typeparam>
    public interface IScriptEnvironment<T>
    {
        public string EngineVersion { get;  }
        public string ContextVersion { get; }
        public T Context { get; }
    }
}