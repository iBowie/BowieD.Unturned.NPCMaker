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
                string typePrefix = Target.GetType().Name;

                if (Target is NPC.IHasUIText hasUiText)
                {
                    return $"{typePrefix} - {hasUiText.UIText}";
                }

                return $"{typePrefix} - {Target}";
            }
        }
    }
}
