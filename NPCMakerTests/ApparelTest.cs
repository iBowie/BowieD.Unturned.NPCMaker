using System;
using BowieD.Unturned.NPCMaker.NPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPCMakerTests
{
    [TestClass]
    public class ApparelTest
    {
        [TestMethod]
        public void InputEveryColor()
        {
            for (byte r = 0; r < 255; r++)
            {
                for (byte g = 0; g < 255; g++)
                {
                    for (byte b = 0; b < 255; b++)
                    {
                        NPCColor color = new NPCColor(r, g, b);
                        string HEX = color.HEX;
                        var HSV = color.HSV;
                    }
                }
            }
        }
        [TestMethod]
        public void InputRandomColor()
        {
            byte[] arr = new byte[3];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(arr);
            NPCColor color = new NPCColor(arr[0], arr[1], arr[2]);
            string hex = color.HEX;
            var hsv = color.HSV;
        }
        [TestMethod]
        public void ReverseColorConvert()
        {
            byte[] arr = new byte[3];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(arr);
            NPCColor color = new NPCColor(arr[0], arr[1], arr[2]);
            string hex = color.HEX;
            NPCColor fromHex = new NPCColor() { HEX = hex };
            var hsv = color.HSV;
            NPCColor fromHSV = NPCColor.FromHSV(hsv.Item1, hsv.Item2, hsv.Item3);
            if (color != fromHex || color != fromHSV)
            {
                throw new Exception("Цвета различаются после конвертации");
            }
        }
    }
}
