using BowieD.Unturned.NPCMaker.GameIntegration.Level;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Parsing;
using BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.UnityTypes;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Devkit
{
    public class Spawnpoint : DevkitHierarchyWorldItem, IHasThumbnail
    {
        public string id;

        public Spawnpoint(string fileName, EGameAssetOrigin origin) : base(fileName, origin)
        {
        }

        DrawingImage createThumbnail(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);

            var info = LevelDatManager.GetInfo(dir);

            ushort border = 1, size = 8;

            if (info != null)
            {
                switch (info.Size)
                {
                    case ELevelSize.TINY:
                        size = 512;
                        border = 16;
                        break;
                    case ELevelSize.SMALL:
                        size = 1024;
                        border = 64;
                        break;
                    case ELevelSize.MEDIUM:
                        size = 2048;
                        border = 64;
                        break;
                    case ELevelSize.LARGE:
                        size = 4096;
                        border = 64;
                        break;
                    case ELevelSize.INSANE:
                        size = 8192;
                        border = 128;
                        break;
                    default:
                        size = 0;
                        break;
                }
            }

            Rect rect = new Rect(new Size(32, 32));

            DrawingGroup g = new DrawingGroup();

            Vector2 levelTo2D(Vector3 pos)
            {
                float num = size - border * 2f;
                return new Vector2(pos.X / num + 0.5f, 0.5f - pos.Z / num);
            }

            var posOnMap = levelTo2D(Position);

            posOnMap.X *= 32;
            posOnMap.Y *= 32;

            BitmapImage bi = null;

            var mapPng = Path.Combine(dir, "Map.png");
            var chartPng = Path.Combine(dir, "Chart.png");

            if (File.Exists(mapPng))
            {
                bi = new BitmapImage();

                bi.BeginInit();

                bi.UriSource = new Uri(mapPng);
                bi.DecodePixelWidth = 32;
                bi.DecodePixelHeight = 32;

                bi.EndInit();
            }
            else if (File.Exists(chartPng))
            {
                bi = new BitmapImage();
                
                bi.BeginInit();
                
                bi.UriSource = new Uri(chartPng);
                bi.DecodePixelWidth = 32;
                bi.DecodePixelHeight = 32;

                bi.EndInit();
            }

            if (bi != null)
                g.Children.Add(new ImageDrawing(bi, rect));

            g.Children.Add(new GeometryDrawing(Brushes.Red, null, new EllipseGeometry(posOnMap, 1, 1)));

            return new DrawingImage(g);
        }

        public override string Name => id;

        private ImageSource _thumb;
        public ImageSource Thumbnail => _thumb;

        protected override void readHierarchyItem(IFileReader reader)
        {
            base.readHierarchyItem(reader);

            id = reader.readValue<string>("ID");

            _thumb = createThumbnail(OriginFileName);
        }
    }
    public class DevkitHierarchyWorldItem : DevkitHierarchyItemBase
    {
        public DevkitHierarchyWorldItem(string fileName, EGameAssetOrigin origin) : base(fileName, origin)
        {
        }

        public Vector3 Position { get; protected set; }

        public override void read(IFileReader reader)
        {
            reader = reader.readObject();
            if (reader != null)
                readHierarchyItem(reader);
        }
        protected virtual void readHierarchyItem(IFileReader reader) 
        {
            Position = reader.readValue<Vector3>("Position");
        }
    }
    public abstract class DevkitHierarchyItemBase : IDevkitHierarchyItem
    {
        public DevkitHierarchyItemBase(string fileName, EGameAssetOrigin origin)
        {
            this.OriginFileName = fileName;
            this.Origin = origin;
        }

        public EGameAssetOrigin Origin { get; }

        public virtual string Name => string.Empty;
        public virtual ushort ID => 0;
        public virtual EIDDef IDDef => EIDDef.FILEORIGIN_DIR_SHORT;
        public virtual Guid GUID => Guid.Empty;

        public string OriginFileName { get; }

        public abstract void read(IFileReader reader);
    }
    public interface IDevkitHierarchyItem : IFileReadable, IAssetPickable, IHasOriginFile { }
}
