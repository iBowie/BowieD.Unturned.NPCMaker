using System.Windows;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public interface IEditable
    {
        void Edit(Window calledFrom);
    }
    public interface ICreatable
    {
        void OnCreate();
    }
    public interface IDeletable
    {
        void OnDelete();
    }
}
