using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Templating.Conditions;
using BowieD.Unturned.NPCMaker.Templating.Conditions.Converters;
using BowieD.Unturned.NPCMaker.Templating.Modify;
using BowieD.Unturned.NPCMaker.Templating.Modify.Converters;
using BowieD.Unturned.NPCMaker.Templating.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Templating
{
    public static class TemplateManager
    {
        static TemplateManager()
        {
            TemplateConditionConverter.Register<TemplateCondition_Input>();
            TemplateConditionConverter.Register<TemplateCondition_AlwaysTrue>();
            TemplateConditionConverter.Register<TemplateCondition_AlwaysFalse>();

            ModifyValueConverter.Register<ModifyValue_String>();
            ModifyValueConverter.Register<ModifyValue_Input>();
            ModifyValueConverter.Register<ModifyValue_Reward>();
            ModifyValueConverter.Register<ModifyValue_Condition>();
            ModifyValueConverter.Register<ModifyValue_Boolean>();
            ModifyValueConverter.Register<ModifyValue_UInt16>();
            ModifyValueConverter.Register<ModifyValue_Modification_Type>();
        }
        public static Template LoadTemplate(string fileName)
        {
            if (File.Exists(fileName))
            {
                string contents = File.ReadAllText(fileName);

                Template template = JsonConvert.DeserializeObject<Template>(contents, new JsonSerializerSettings()
                {
                    Converters = new JsonConverter[]
                    {
                        new TemplateConditionConverter(),
                        new ModifyValueConverter(),
                        new StringEnumConverter()
                    }
                });

                return template;
            }
            else
            {
                return null;
            }
        }
        public static void AskForInput(Template template)
        {
            MultiFieldInputView_Dialog multiField = new MultiFieldInputView_Dialog(template.Inputs.Values.Select(d => d.Default.ToString()).ToArray());
            
            string[] texts = template.Inputs.Select(kv => kv.Value.Text ?? kv.Key).ToArray();
            string[] tooltips = template.Inputs.Select(kv => kv.Value.ToolTip ?? string.Empty).ToArray();

            if (multiField.ShowDialog(texts, template.Name, tooltips) == true)
            {
                var values = multiField.Values;

                for (int i = 0; i < values.Length; i++)
                {
                    var kv = template.Inputs.ElementAt(i);

                    template.UserInputs[kv.Key] = Convert.ChangeType(values[i], TypeResolver.Resolve(kv.Value.Type, null, template));
                }
            }
        }
        public static void PrepareTemplate(Template template)
        {
            template.Init();
        }
        public static object ApplyTemplate(Template template)
        {
            template.Apply();

            return template.FinalObject;
        }
        public static bool IsCorrectContext(Template template, Type type)
        {
            return TypeResolver.Resolve(template.Type, null, template) == type;
        }
    }
}
