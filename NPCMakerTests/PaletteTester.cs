using System;
using System.Collections.Generic;
using BowieD.Unturned.NPCMaker.Coloring;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPCMakerTests
{
    [TestClass]
    public class PaletteTester
    {
        private static Dictionary<(byte R, byte G, byte B), (double C, double M, double Y, double K)> cmykToRgb = new Dictionary<(byte R, byte G, byte B), (double C, double M, double Y, double K)>()
        {
            { (0,0,0), (0,0,0,1) },
            { (255,255,255), (0,0,0,0) },
            { (255,0,0), (0,1,1,0) },
            { (0, 255, 0), (1, 0, 1, 0) },
            { (0,0,255), (1,1,0,0) },
            { (255, 255, 0), (0, 0, 1, 0) },
            { (0,255,255), (1,0,0,0) },
            { (255,0,255), (0,1,0,0) }
        };
        private static Dictionary<(byte R, byte G, byte B), (double H, double S, double L)> hslToRgb = new Dictionary<(byte R, byte G, byte B), (double, double, double)>()
        {
            { (0,0,0)      , (0,0,0)      },
            { (255,255,255), (0,0,1)      },
            { (255,0,0)    , (0,1,0.5)    },
            { (0,255,0)    , (120,1,0.5)  },
            { (0,0,255)    , (240,1,0.5)  },
            { (255,255,0)  , (60,1,0.5)    },
            { (0,255,255)  , (180,1,0.5)  },
            { (255,0,255)  , (300,1,0.5)  },
            { (191,191,191), (0,0,0.75)   },
            { (127,127,127), (0,0,0.5)    },
            { (127,0,0)    , (0,1,0.25)   },
            { (127,127,0)  , (60,1,0.25)  },
            { (0,127,0)    , (120,1,0.25) },
            { (127,0,127)  , (300,1,0.25) },
            { (0,127,127)  , (180,1,0.25) },
            { (0,0,127)    , (240,1,0.25) }
        };
        private static Dictionary<(byte R, byte G, byte B), (double H, double S, double V)> hsvToRgb = new Dictionary<(byte R, byte G, byte B), (double H, double S, double V)>()
        {
            {(0,0,0)      , (0,0,0)          },
            {(255,255,255), (0,0,1)          },
            {(255,0,0)    , (0,1,1)          },
            {(0,255,0)    , (120,1,1)    },
            {(0,0,255)    , (240,1,1)    },
            {(255,255,0)  , (60,1,1)         },
            {(0,255,255)  , (180,1,1)        },
            {(255,0,255)  , (300,1,1)        },
            {(191,191,191), (0,0,0.75)   },
            {(128,128,128), (0,0,0.5)      },
            {(128,0,0)    , (0,1,0.5)        },
            {(128,128,0)  , (60,1,0.5)   },
            {(0,128,0)    , (120,1,0.5)  },
            {(128,0,128)  , (300,1,0.5)  },
            {(0,128,128)  , (180,1,0.5)  },
            {(0,0,128)    , (240,1,0.5) }
        };

        [TestMethod]
        public void TestCMYKtoRGB()
        {
            foreach (var k in cmykToRgb)
            {
                var CMYK = k.Value;
                var rgbResult = new PaletteCMYK
                {
                    CMYK = CMYK
                }.ToRGB();
                if (rgbResult != k.Key)
                    throw new Exception($"Wrong color conversion (CMYK -> RGB). Expected: {k.Key}. Got: {rgbResult}");
            }
        }
        [TestMethod]
        public void TestRGBtoCMYK()
        {
            foreach (var k in cmykToRgb)
            {
                var RGB = k.Key;
                var cmykResult = (new PaletteCMYK().FromRGB(RGB) as PaletteCMYK).CMYK;
                if (cmykResult != k.Value)
                    throw new Exception($"Wrong color conversion (RGB -> CMYK). Expected: {k.Value}. Got: {cmykResult}");
            }
        }
        [TestMethod]
        public void TestHSLtoRGB()
        {
            foreach (var k in hslToRgb)
            {
                var HSL = k.Value;
                var rgbResult = new PaletteHSL
                {
                    HSL = HSL
                }.ToRGB();
                if (rgbResult != k.Key)
                    throw new Exception($"Wrong color conversion (HSL -> RGB). Expected: {k.Key}. Got: {rgbResult}");
            }
        }
        [TestMethod]
        public void TestRGBtoHSL()
        {
            foreach (var k in hslToRgb)
            {
                var RGB = k.Key;
                var hslResult = (new PaletteHSL().FromRGB(RGB) as PaletteHSL).HSL;
                if (hslResult != k.Value)
                    throw new Exception($"Wrong color conversion (RGB -> HSL). Expected: {k.Value}. Got: {hslResult}");
            }
        }
        [TestMethod]
        public void TestHSVtoRGB()
        {
            foreach (var k in hsvToRgb)
            {
                var HSV = k.Value;
                var rgbResult = new PaletteHSV
                {
                    HSV = HSV
                }.ToRGB();
                if (rgbResult != k.Key)
                    throw new Exception($"Wrong color conversion (HSV -> RGB). Expected: {k.Key}. Got: {rgbResult}");
            }
        }
        [TestMethod]
        public void TestRGBtoHSV()
        {
            foreach (var k in hsvToRgb)
            {
                var RGB = k.Key;
                var hsvResult = (new PaletteHSV().FromRGB(RGB) as PaletteHSV).HSV;
                if (hsvResult != k.Value)
                    throw new Exception($"Wrong color conversion (RGB -> HSV). Expected: {k.Value}. Got: {hsvResult}");
            }
        }
    }
}
