namespace PolicyExample.Scripting.Jint
{
    public class ClearScriptScript 
    {
        public ClearScriptScript(string javaScriptCode)
        {
            JavaScriptCode = javaScriptCode;
        }

        public string JavaScriptCode { get;  }
        string ContextTypeName { get; }
        int ContextVersion { get; }
        string EngineVersion { get; }
        string ContextSchema { get; }
    }
}