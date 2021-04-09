namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public interface IHasOriginFile : IAssetPickable
    {
        string OriginFileName { get; }
    }
}
