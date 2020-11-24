using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public interface IFileReadable
    {
        void read(IFileReader reader);
    }
}
