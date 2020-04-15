using Newtonsoft.Json;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed class AppPackage
    {
        public AppPackage()
        {
            Credits = new Dictionary<string, string>();
            Guides = new Dictionary<string, string>();
        }

        public const string url = "https://raw.githubusercontent.com/iBowie/BowieD.Unturned.NPCMaker/master/PACKAGE.json";

        private string[] _patrons;
        [JsonProperty("patrons")]
        public string[] Patrons
        {
            get => _patrons ?? new string[0];
            set => _patrons = value;
        }
        private Holiday[] _holidays;
        [JsonProperty("holidays")]
        public Holiday[] Holidays
        {
            get => _holidays ?? new Holiday[0];
            set => _holidays = value;
        }
        private Dictionary<string, string> _credits;
        [JsonProperty("credits")]
        public Dictionary<string, string> Credits
        {
            get => _credits ?? new Dictionary<string, string>();
            set => _credits = value;
        }
        private Dictionary<string, string> _guides;
        [JsonProperty("guides")]
        public Dictionary<string, string> Guides
        {
            get => _guides ?? new Dictionary<string, string>();
            set => _guides = value;
        }
        private FeedbackLink[] _feedback;
        [JsonProperty("feedback")]
        public FeedbackLink[] FeedbackLinks
        {
            get => _feedback ?? new FeedbackLink[0];
            set => _feedback = value;
        }
    }
}
