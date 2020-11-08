namespace PolicyExample.Tests
{
    public class JintScript : IJintScript
    {
        public JintScript(string javaScriptCode)
        {
            JavaScriptCode = javaScriptCode;
        }

        public string JavaScriptCode { get;  }
    }
}