namespace UnityAuxiliaryTools.Trigger.Factory
{
    public class TriggerFactory : ITriggerFactory
    {
        public IRegularTrigger CreateRegularTrigger()
        {
            return new RegularTrigger();
        }
    }
}