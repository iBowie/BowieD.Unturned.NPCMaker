using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameNPCAsset : GameObjectAsset, IHasThumbnail
    {
        public GameNPCAsset(NPCCharacter character, EGameAssetOrigin origin) : base(Guid.Parse(character.GUID), origin)
        {
            this.id = character.ID;
            this.name = character.EditorName;

            this.character = character;
        }
        public GameNPCAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, name, id, guid, type, origin)
        {
            character = new Parsing.ParseTool(data, local).ParseCharacter();
        }

        public NPCCharacter character;

        private readonly Size size = new Size(32, 32);

        public ImageSource Thumbnail
        {
            get
            {
                Rect rect = new Rect(size);

                DrawingGroup group = new DrawingGroup();

                group.Children.Add(new GeometryDrawing(character.skinColor, new Pen(character.skinColor, 1), new RectangleGeometry(rect)));
                
                if (character.face < 32)
                    group.Children.Add(new ImageDrawing(new BitmapImage(new Uri($"pack://application:,,,/Resources/Unturned/Faces/{character.face}.png")), rect));

                return new DrawingImage(group);
            }
        }
    }
}
