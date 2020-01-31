using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class ConfigCommand : Command
    {
        private class ConfigFieldInfo
        {
            public ConfigFieldInfo(FieldInfo field)
            {
                Name = field.Name;
                Type = field.FieldType;
                fInfo = field;
            }
            public string Name { get; }
            public Type Type { get; }
            public object Value
            {
                get
                {
                    return fInfo.GetValue(AppConfig.Instance);
                }
                set
                {
                    fInfo.SetValue(AppConfig.Instance, value);
                    AppConfig.Instance.Save();
                }
            }
            private FieldInfo fInfo;
        }
        static HashSet<ConfigFieldInfo> Fields { get; }
        static ConfigCommand()
        {
            Fields = new HashSet<ConfigFieldInfo>();
            var cfgType = typeof(AppConfig);
            foreach (var field in cfgType.GetFields())
            {
                if (field.IsStatic)
                    continue;
                Fields.Add(new ConfigFieldInfo(field));
            }
        }
        public override string Name => "config";
        public override string Help => "Manually edit configuration";
        public override string Syntax => "<set/get/list/reset> [field] [value]";
        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "set" when args.Length > 2:
                        {
                            foreach (var field in Fields)
                            {
                                if (field.Name == args[1])
                                {
                                    try
                                    {
                                        object newValue;
                                        if (field.Type.IsEnum)
                                        {
                                            newValue = Enum.Parse(field.Type, args[2]);
                                        }
                                        else
                                        {
                                            newValue = Convert.ChangeType(args[2], field.Type);
                                        }
                                        field.Value = newValue;
                                        App.Logger.Log($"[ConfigCommand]/set - .{field.Name} set to {field.Value}");
                                        App.Logger.Log($"[ConfigCommand]/set - Restart the app to apply changes.");
                                    }
                                    catch
                                    {
                                        App.Logger.Log($"[ConfigCommand]/set - Can't convert {args[2]} to {field.Type.Name}");
                                    }
                                    return;
                                }
                            }
                            App.Logger.Log($"[ConfigCommand]/set - Can't find field with name {args[1]}");
                            return;
                        }
                    case "get" when args.Length > 1:
                        {
                            foreach (var field in Fields)
                            {
                                if (field.Name == args[1])
                                {
                                    App.Logger.Log($"[ConfigCommand]/get - .{field.Name} ({field.Type.Name}) - {field.Value}");
                                    return;
                                }
                            }
                            App.Logger.Log($"[ConfigCommand]/get - Can't find field with name {args[1]}");
                            return;
                        }
                    case "list":
                        {
                            foreach (var field in Fields)
                            {
                                App.Logger.Log($"[ConfigCommand]/list - .{field.Name} ({field.Type.Name})");
                            }
                            return;
                        }
                    case "reset":
                        {
                            AppConfig.Instance.LoadDefaults();
                            AppConfig.Instance.Save();
                            App.Logger.Log($"[ConfigCommand]/reset - Restart the app to apply changes.");
                            return;
                        }
                }
            }
            App.Logger.Log($"[ConfigCommand] - Use {Name} {Syntax}");
        }
    }
}
