namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public enum EGameAssetOrigin
    {
        Unturned = 1 << 0,
        Workshop = 1 << 1,
        Project = 1 << 2,

        Game = Unturned | Workshop
    }
}
