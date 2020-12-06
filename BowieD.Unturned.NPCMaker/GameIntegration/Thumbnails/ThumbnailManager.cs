using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public static class ThumbnailManager
    {
        private static readonly Dictionary<string, BitmapImage> _results = new Dictionary<string, BitmapImage>();

        public static void CreateThumbnail(Uri uri)
        {
            if (_results.ContainsKey(uri.AbsoluteUri))
                return;

            var bmp = new BitmapImage();

            bmp.BeginInit();

            bmp.UriSource = uri;
            bmp.DecodePixelHeight = 32;
            bmp.CacheOption = BitmapCacheOption.OnLoad;

            bmp.EndInit();

            _results[uri.AbsoluteUri] = bmp;
        }

        public static BitmapImage GetThumbnail(Uri uri)
        {
            if (!_results.ContainsKey(uri.AbsoluteUri))
                CreateThumbnail(uri);

            return _results[uri.AbsoluteUri];
        }
    }
}
