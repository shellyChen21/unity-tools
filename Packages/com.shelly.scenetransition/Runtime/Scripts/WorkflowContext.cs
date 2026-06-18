namespace Shelly.SceneTransition
{
    public class WorkflowContext
    {
        public readonly SceneTransitionBehaviour FaderPrefab;

        public SceneTransitionBehaviour CurrentFader;

        public WorkflowContext(SceneTransitionBehaviour faderPrefab)
        {
            FaderPrefab = faderPrefab;
        }
    }
}
