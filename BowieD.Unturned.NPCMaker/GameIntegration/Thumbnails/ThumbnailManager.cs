using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public static class ThumbnailManager
    {
        private static readonly Dictionary<string, BitmapImage> _results = new Dictionary<string, BitmapImage>();

        public static BitmapImage CreateThumbnail(Uri uri)
        {
            if (_results.TryGetValue(uri.AbsoluteUri, out var pre))
                return pre;

            var bmp = new BitmapImage();

            bmp.BeginInit();

            bmp.UriSource = uri;
            bmp.DecodePixelHeight = 32;
            bmp.CacheOption = BitmapCacheOption.OnLoad;

            bmp.EndInit();

            _results[uri.AbsoluteUri] = bmp;

            return bmp;
        }

        public static void Purge()
        {
            _results.Clear();
        }
    }
}
