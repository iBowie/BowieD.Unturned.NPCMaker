using BowieD.Unturned.NPCMaker.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        private Notification[] _notifications;
        [JsonProperty("notifications")]
        public Notification[] Notifications
        {
            get => _notifications ?? new Notification[0];
            set => _notifications = value;
        }

        [JsonObject(MemberSerialization.OptIn)]
        public sealed class FeedbackLink
        {
            [JsonProperty("icon")]
            public string Icon { get; set; }
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("loc")]
            public bool Localize { get; set; }
            [JsonProperty("url")]
            public string URL { get; set; }
        }
        [JsonObject(MemberSerialization.OptIn)]
        public sealed class Holiday
        {
            [JsonConstructor]
            public Holiday(string notification, DateTimeRange range)
            {
                Notification = notification;
                Range = range;
            }
            [JsonProperty("notification")]
            public string Notification { get; }
            [JsonProperty("range")]
            public DateTimeRange Range { get; }
        }
        [JsonObject(MemberSerialization.OptIn)]
        public sealed class Notification
        {
            [JsonProperty("loc")]
            public bool Localize { get; set; }
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("buttons")]
            public Button[] Buttons
            {
                get => _buttons ?? new Button[0];
                set => _buttons = value ?? new Button[0];
            }
            private Button[] _buttons;


            [JsonObject(MemberSerialization.OptIn)]
            public sealed class Button
            {
                [JsonProperty("loc")]
                public bool Localize { get; set; }
                [JsonProperty("text")]
                public string Text { get; set; }

                [JsonProperty("actionType")]
                [JsonConverter(typeof(StringEnumConverter))]
                public EButtonAction Action { get; set; }
                [JsonProperty("actionArgs")]
                public string ActionArgs { get; set; }

                public enum EButtonAction
                {
                    NONE,
                    COMMAND,
                    URL
                }

                public Action GetButtonAction()
                {
                    switch (Action)
                    {
                        case EButtonAction.COMMAND:
                            return () =>
                            {
                                Command.Execute(ActionArgs);
                            };
                        case EButtonAction.URL:
                            return () =>
                            {
                                Process.Start(ActionArgs);
                            };
                        case EButtonAction.NONE:
                        default:
                            return () => { };
                    }
                }
            }
        }
    }
}
