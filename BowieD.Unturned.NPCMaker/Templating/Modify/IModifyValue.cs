namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    public interface IModifyValue
    {
        object GetObject(Template template);
        object Value { get; set; }
        IModifyValue[] Parameter { get; set; }
        ModifyEntry[] Modify { get; set; }
    }
}