namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public struct FindReplaceTarget
    {
        public FindReplaceTarget(object target, ReplaceableProperty replaceableProperty, IFindReplacerTargeter targeter)
        {
            Target = target;
            ReplaceableProperty = replaceableProperty;
            Targeter = targeter;
        }

        public object Target { get; }
        public ReplaceableProperty ReplaceableProperty { get; }
        public IFindReplacerTargeter Targeter { get; }

        public string TargetDisplayName
        {
            get
            {
                if (Target is NPC.IHasUIText hasUiText)
                {
                    return hasUiText.UIText;
                }

                return Target.ToString();
            }
        }
    }
}
