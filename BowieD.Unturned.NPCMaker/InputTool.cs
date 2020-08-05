using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker
{
    public static class InputTool
    {
        public static bool IsKeyDown(Key key)
        {
            return Keyboard.IsKeyDown(key);
        }
        public static bool IsKeyUp(Key key)
        {
            return Keyboard.IsKeyUp(key);
        }
        public static bool IsKeyToggled(Key key)
        {
            return Keyboard.IsKeyToggled(key);
        }
    }
}
