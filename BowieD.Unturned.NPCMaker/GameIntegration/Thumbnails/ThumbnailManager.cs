using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public static class ThumbnailManager
    {
        private static readonly Dictionary<string, BitmapImage> _results = new Dictionary<string, BitmapImage>();

        internal static int GeneratedThumbnailCount { get; set; }

        public static BitmapImage CreateThumbnail(Uri uri)
        {
            if (_results.TryGetValue(uri.AbsoluteUri, out var pre))
                return pre;

            var bmp = new BitmapImage();

            bmp.BeginInit();

            bmp.UriSource = uri;
            bmp.DecodePixelHeight = 32;

            if (AppConfig.Instance.generateThumbnailsBeforehand)
                bmp.CacheOption = BitmapCacheOption.OnLoad;
            else
                bmp.CacheOption = BitmapCacheOption.OnDemand;

            bmp.EndInit();

            _results[uri.AbsoluteUri] = bmp;
            GeneratedThumbnailCount++;

            return bmp;
        }

        public static void Purge()
        {
            _results.Clear();
            GeneratedThumbnailCount = 0;
        }
    }
}
