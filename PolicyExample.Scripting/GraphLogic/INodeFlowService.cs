namespace PolicyExample.Scripting.GraphLogic
{
    public interface INodeFlowService
    {
        void Stop();
        void RedirectToParent();
        bool RedirectToChild(int index);
    }
}