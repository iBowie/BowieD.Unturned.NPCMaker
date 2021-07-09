namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public enum EGameAssetOrigin
    {
        Unturned = 1 << 0,
        Workshop = 1 << 1,
        Project = 1 << 2,
        Hooked = 1 << 3,

        Game = Unturned | Workshop
    }
}
