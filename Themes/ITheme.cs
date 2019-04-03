namespace BowieD.Unturned.NPCMaker
{
    public interface ITheme
    {
        void Apply();
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
    }
}