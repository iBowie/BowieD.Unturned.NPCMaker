using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public abstract class IFindReplacerTargeter
    {
        private readonly List<ReplaceableProperty> replaceableProperties;

        protected abstract IEnumerable<ReplaceableProperty> CreateReplaceableProperties();

        public virtual bool CanGoToTarget => false;
        public virtual bool ClosesWhenGoesToTarget => true;
        public virtual void GoToTarget(object target) { }

        public abstract IEnumerable<object> GetTargets();

        public IFindReplacerTargeter()
        {
            replaceableProperties = new List<ReplaceableProperty>(CreateReplaceableProperties());
        }

        public IEnumerable<ReplaceableProperty> ReplaceableProperties => replaceableProperties;
    }
}
