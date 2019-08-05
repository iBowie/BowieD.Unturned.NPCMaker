using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.NPCMaker.Configuration.Attributes
{
    public sealed class ConfigurationFieldAttribute : Attribute
    {
        public string tabId { get; private set; }
        public ConfigurationFieldAttribute(string tabId) { this.tabId = tabId; }
    }
}
