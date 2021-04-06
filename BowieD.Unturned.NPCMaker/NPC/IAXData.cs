using System;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public interface IAXData
    {
        void Load(XmlNode node, int version);
        void Save(XmlDocument document, XmlNode node);
    }
    public interface IAXDataDerived<out T> : IAXData
    {
        Func<XmlNode, int, T> CreateFromNodeFunction { get; }
        string TypeName { get; }
    }
}
