using System;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Achievements
{
    public static class ImposterClass
    {
        public static void Play()
        {
            Task.Run(() => // this code is good it's making tasks
            {
                Console.Beep(262, 400);
                Console.Beep(330, 200);
                Console.Beep(349, 200);
                Console.Beep(370, 200);
                Console.Beep(349, 200);
                Console.Beep(330, 200);
                Console.Beep(262, 800);
                Console.Beep(247, 100);
                Console.Beep(294, 100);
                Console.Beep(262, 400);
            });
        }
    }
}
