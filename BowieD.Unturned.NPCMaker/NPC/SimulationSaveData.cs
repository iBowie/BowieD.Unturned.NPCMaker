using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Data;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class SimulationSaveData : AXData<Simulation>
    {
        public SimulationSaveData(string fileName)
        {
            this.FileName = fileName;
        }

        public override string FileName { get; }
        public override string RootNodeName => "SimulationData";
        public override int CurrentSaveDataVersion => 1;
        protected override void GetRootAndVersion(XmlDocument document, out XmlNode root, out int version)
        {
            root = document[RootNodeName];
            version = root[SaveDataNodeName].ToInt32();
        }
    }
}
