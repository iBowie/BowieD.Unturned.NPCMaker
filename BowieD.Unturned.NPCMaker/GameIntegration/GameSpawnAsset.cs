using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameSpawnAsset : GameAsset, IHasAnimatedThumbnail, IHasToolTip
    {
        public class SpawnTable
        {
            public ushort assetID;
            public ushort spawnID;
            public int weight;
            public float chance;
            public bool isLink;
            public bool isOverride;
            public bool hasNotifiedChild;
        }

        public GameSpawnAsset(Guid guid, EGameAssetOrigin origin) : base(guid, origin)
        {
        }

        public GameSpawnAsset(DataReader data, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            int num = data.ReadInt32("Roots");
            insertRoots = new List<SpawnTable>(num);
            for (int i = 0; i < num; i++)
            {
                SpawnTable spawnTable = new SpawnTable();
                spawnTable.spawnID = data.ReadUInt16($"Root_{i}_Spawn_ID", 0);
                spawnTable.isOverride = data.Has($"Root_{i}_Override");
                spawnTable.weight = data.ReadInt32($"Root_{i}_Weight", spawnTable.isOverride ? 1 : 0);
                spawnTable.chance = 0f;
                insertRoots.Add(spawnTable);
            }
            _roots = new List<SpawnTable>(num);
            int num2 = data.ReadInt32("Tables");
            _tables = new List<SpawnTable>(num2);
            for (int j = 0; j < num2; j++)
            {
                SpawnTable spawnTable2 = new SpawnTable
                {
                    assetID = data.ReadUInt16($"Table_{j}_Asset_ID", 0),
                    spawnID = data.ReadUInt16($"Table_{j}_Spawn_ID", 0),
                    weight = data.ReadInt32($"Table_{j}_Weight"),
                    chance = 0f
                };
                tables.Add(spawnTable2);
            }
            areTablesDirty = true;
        }

        protected List<SpawnTable> _roots;
        protected List<SpawnTable> _tables;

        public List<SpawnTable> insertRoots { get; protected set; }
        public List<SpawnTable> roots => _roots;
        public List<SpawnTable> tables => _tables;
        public bool areTablesDirty { get; protected set; }

        public override EGameAssetCategory Category => EGameAssetCategory.SPAWN;

        public IEnumerable<ImageSource> Thumbnails
        {
            get
            {
                HashSet<ushort> poss = new HashSet<ushort>();
                resolveAllPossibilites<GameItemAsset>(poss);

                foreach (var p in poss)
                {
                    if (GameAssetManager.TryGetAsset<GameItemAsset>(p, out var itemAsset))
                    {
                        yield return ThumbnailManager.CreateThumbnail(itemAsset.ImagePath);
                    }
                }
            }
        }

        public UIElement ToolTipContent
        {
            get
            {
                var thumbs = Thumbnails;

                WrapPanel wp = new WrapPanel();

                foreach (var t in thumbs)
                {
                    wp.Children.Add(new Image()
                    {
                        Source = t,
                        Width = 32,
                        Height = 32,
                        Margin = new Thickness(1)
                    });
                }

                return wp;
            }
        }

        public void resolve(out ushort id, out bool isSpawn)
        {
            id = 0;
            isSpawn = false;
            if (tables.Count < 1)
            {
                return;
            }
            if (areTablesDirty)
            {
                sortAndNormalizeWeights();
            }
            float value = Random.Value;
            int num = 0;
            while (true)
            {
                if (num >= tables.Count)
                {
                    return;
                }
                if (value < tables[num].chance || num == tables.Count - 1)
                {
                    if (tables[num].spawnID != 0)
                    {
                        id = tables[num].spawnID;
                        isSpawn = true;
                        return;
                    }
                    if (tables[num].assetID != 0)
                    {
                        break;
                    }
                }
                num++;
            }
            id = tables[num].assetID;
            isSpawn = false;
        }
        public void sortAndNormalizeWeights()
        {
            if (areTablesDirty)
            {
                areTablesDirty = false;
                tables.Sort((a, b) => b.weight - a.weight);
                float num = 0f;
                foreach (SpawnTable table in tables)
                {
                    num += table.weight;
                }
                float num2 = 0f;
                foreach (SpawnTable table2 in tables)
                {
                    num2 += table2.weight;
                    table2.chance = num2 / num;
                }
            }
        }

        public void resolveAllPossibilites<T>(HashSet<ushort> possibilites) where T : GameAsset
        {
            foreach (var t in tables)
            {
                if (t.spawnID > 0 && GameAssetManager.TryGetAsset<GameSpawnAsset>(t.spawnID, out var gameSpawnAsset))
                {
                    gameSpawnAsset.sortAndNormalizeWeights();

                    gameSpawnAsset.resolveAllPossibilites<T>(possibilites);
                }
                else if (t.assetID > 0 && GameAssetManager.TryGetAsset<T>(t.assetID, out var gameAsset))
                {
                    possibilites.Add(gameAsset.id);
                }
            }
        }
    }
}
