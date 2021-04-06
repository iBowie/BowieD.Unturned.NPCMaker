using BowieD.Unturned.NPCMaker.Coloring;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.Common
{
    public static class XmlNodeExtension
    {
        private static CultureInfo cltr = CultureInfo.InvariantCulture;

        #region Read
        public static sbyte ToSByte(this XmlNode node)
        {
            return sbyte.Parse(node.InnerText, cltr);
        }
        public static byte ToByte(this XmlNode node)
        {
            return byte.Parse(node.InnerText, cltr);
        }
        public static short ToInt16(this XmlNode node)
        {
            return short.Parse(node.InnerText, cltr);
        }
        public static ushort ToUInt16(this XmlNode node)
        {
            return ushort.Parse(node.InnerText, cltr);
        }
        public static int ToInt32(this XmlNode node)
        {
            return int.Parse(node.InnerText, cltr);
        }
        public static uint ToUInt32(this XmlNode node)
        {
            return uint.Parse(node.InnerText, cltr);
        }
        public static long ToInt64(this XmlNode node)
        {
            return long.Parse(node.InnerText, cltr);
        }
        public static ulong ToUInt64(this XmlNode node)
        {
            return ulong.Parse(node.InnerText, cltr);
        }
        public static float ToSingle(this XmlNode node)
        {
            return float.Parse(node.InnerText, cltr);
        }
        public static double ToDouble(this XmlNode node)
        {
            return double.Parse(node.InnerText, cltr);
        }
        public static T ToEnum<T>(this XmlNode node)
        {
            return (T)Enum.Parse(typeof(T), node.InnerText);
        }
        public static bool ToBoolean(this XmlNode node)
        {
            return bool.Parse(node.InnerText);
        }
        public static Color ToColor(this XmlNode node, int version)
        {
            Color clr = new Color();

            clr.R = node["R"].ToByte();
            clr.G = node["G"].ToByte();
            clr.B = node["B"].ToByte();

            return clr;
        }

        #region Nullables
        public static byte? ToNullableByte(this XmlNode node)
        {
            var a = node.Attributes["xsi:nil"];
            if (a != null && a.Value == "true")
                return null;

            return node.ToByte();
        }
        public static ushort? ToNullableUInt16(this XmlNode node)
        {
            var a = node.Attributes["xsi:nil"];
            if (a != null && a.Value == "true")
                return null;

            return node.ToUInt16();
        }
        public static int? ToNullableInt32(this XmlNode node)
        {
            var a = node.Attributes["xsi:nil"];
            if (a != null && a.Value == "true")
                return null;

            return node.ToInt32();
        }
        #endregion
        #endregion
        #region Collections
        public static IEnumerable<T> ParseAXDataCollection<T>(this XmlNode node, int version, Func<XmlNode, int, T> factory = null) where T : IAXData, new()
        {
            Func<XmlNode, int, T> create;

            if (factory == null)
            {
                create = new Func<XmlNode, int, T>((pn, vers) =>
                {
                    return new T();
                });
            }
            else
            {
                var dtype = typeof(IAXDataDerived<T>);

                if (dtype.IsAssignableFrom(typeof(T)))
                {
                    var inst = new T();

                    create = (inst as IAXDataDerived<T>).CreateFromNodeFunction;
                }

                create = factory;
            }

            foreach (XmlNode cNode in node.ChildNodes)
            {
                var el = create(cNode, version);

                el.Load(cNode, version);

                yield return el;
            }
        }
        public static IEnumerable<string> ParseStringCollection(this XmlNode node)
        {
            foreach (XmlNode cNode in node.ChildNodes)
            {
                yield return cNode.InnerText;
            }
        }
        public static IEnumerable<int> ParseInt32Collection(this XmlNode node)
        {
            foreach (XmlNode cNode in node.ChildNodes)
            {
                yield return cNode.ToInt32();
            }
        }
        public static XmlNode WriteAXDataCollection<T>(this XmlNode node, XmlDocument doc, string itemName, IEnumerable<T> ts) where T : IAXData
        {
            foreach (var elem in ts)
            {
                var cNode = doc.CreateNodeC(itemName, node);

                elem.Save(doc, cNode);
            }

            return node;
        }
        public static XmlNode WriteStringCollection(this XmlNode node, XmlDocument doc, IEnumerable<string> ts, string itemName = "string")
        {
            foreach (var elem in ts)
            {
                doc.CreateNodeC(itemName, node).WriteString(elem);
            }

            return node;
        }
        public static XmlNode WriteInt32Collection(this XmlNode node, XmlDocument doc, IEnumerable<int> ts, string itemName = "int")
        {
            foreach (var elem in ts)
            {
                doc.CreateNodeC(itemName, node).WriteInt32(elem);
            }

            return node;
        }
        #endregion
        #region Write
        public static XmlNode WriteByte(this XmlNode node, byte value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteInt16(this XmlNode node, short value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }

        public static XmlNode WriteInt32(this XmlNode node, int value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteInt64(this XmlNode node, long value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteUInt16(this XmlNode node, ushort value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteUInt32(this XmlNode node, uint value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteUInt64(this XmlNode node, ulong value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteSingle(this XmlNode node, float value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteDouble(this XmlNode node, double value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteBoolean(this XmlNode node, bool value)
        {
            node.InnerText = value.ToString(cltr);

            return node;
        }
        public static XmlNode WriteString(this XmlNode node, string value)
        {
            node.InnerText = value;

            return node;
        }
        public static XmlNode WriteEnum<T>(this XmlNode node, T value) where T : Enum
        {
            node.InnerText = Enum.GetName(typeof(T), value);

            return node;
        }
        public static XmlNode WriteColor(this XmlNode node, Color value)
        {
            var doc = node.OwnerDocument;

            doc.CreateNodeC("R", node).WriteByte(value.R);
            doc.CreateNodeC("G", node).WriteByte(value.G);
            doc.CreateNodeC("B", node).WriteByte(value.B);

            return node;
        }

        #region Nullables
        public static XmlNode WriteNullableByte(this XmlNode node, byte? value)
        {
            if (value.HasValue)
                return node.WriteByte(value.Value);

            var a = node.OwnerDocument.CreateAttribute("xsi:nil");
            a.Value = "true";

            node.Attributes.Append(a);

            return node;
        }
        public static XmlNode WriteNullableUInt16(this XmlNode node, ushort? value)
        {
            if (value.HasValue)
                return node.WriteUInt16(value.Value);

            var a = node.OwnerDocument.CreateAttribute("xsi:nil");
            a.Value = "true";

            node.Attributes.Append(a);

            return node;
        }
        public static XmlNode WriteNullableInt32(this XmlNode node, int? value)
        {
            if (value.HasValue)
                return node.WriteInt32(value.Value);

            var a = node.OwnerDocument.CreateAttribute("xsi:nil");
            a.Value = "true";

            node.Attributes.Append(a);

            return node;
        }
        #endregion
        #endregion

        public static XmlNode CreateNodeC(this XmlDocument doc, string name, XmlNode parent)
        {
            var node = doc.CreateElement(name);

            if (parent != null)
                parent.AppendChild(node);

            return node;
        }
        public static XmlAttribute CreateAttributeC(this XmlDocument doc, string name, XmlNode parent)
        {
            var a = doc.CreateAttribute(name);

            if (parent != null)
                parent.Attributes.Append(a);

            return a;
        }
    }
}
