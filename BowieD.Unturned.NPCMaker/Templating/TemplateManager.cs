using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Currency;
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

            #region Complex
            ModifyValueConverter.Register<ModifyValue_String>();
            ModifyValueConverter.Register<ModifyValue_Input>();
            ModifyValueConverter.Register<ModifyValue_Reward>();
            ModifyValueConverter.Register<ModifyValue_Condition>();

            ModifyValueConverter.Register<ModifyValue_Parameterless>("character", typeof(NPCCharacter));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("dialogue", typeof(NPCDialogue));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("vendor", typeof(NPCVendor));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("quest", typeof(NPCQuest));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("vendoritem", typeof(VendorItem));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("clothing", typeof(NPCClothing));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("message", typeof(NPCMessage));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("response", typeof(NPCResponse));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("project", typeof(NPCProject));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("currency", typeof(CurrencyAsset));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("currencyentry", typeof(CurrencyEntry));
            #endregion
            #region Primitives
            ModifyValueConverter.Register<ModifyValue_Parameterless>("bool", typeof(bool));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("sbyte", typeof(sbyte));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("byte", typeof(byte));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("short", typeof(short));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ushort", typeof(ushort));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("int", typeof(int));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("uint", typeof(uint));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("long", typeof(long));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ulong", typeof(ulong));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("float", typeof(float));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("double", typeof(double));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("decimal", typeof(decimal));

            ModifyValueConverter.Register<ModifyValue_Parameterless>("bool?", typeof(bool?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("sbyte?", typeof(sbyte?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("byte?", typeof(byte?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("short?", typeof(short?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ushort?", typeof(ushort?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("int?", typeof(int?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("uint?", typeof(uint?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("long?", typeof(long?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ulong?", typeof(ulong?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("float?", typeof(float?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("double?", typeof(double?));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("decimal?", typeof(decimal?));
            #endregion
            #region Enums
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Clothing_Type", typeof(Clothing_Type));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ParseType", typeof(ParseType));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ELanguage", typeof(ELanguage));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ENPCHoliday", typeof(ENPCHoliday));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Operation_Type", typeof(Operation_Type));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ESkillset", typeof(ESkillset));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Zombie_Type", typeof(Zombie_Type));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Equip_Type", typeof(Equip_Type));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Quest_Status", typeof(Quest_Status));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("RewardType", typeof(RewardType));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("NPC_Pose", typeof(NPC_Pose));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ItemType", typeof(ItemType));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Logic_Type", typeof(Logic_Type));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Condition_Type", typeof(Condition_Type));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("Modification_Type", typeof(Modification_Type));
            #endregion
            #region Arrays
            ModifyValueConverter.Register<ModifyValue_Parameterless>("bool[]", typeof(bool[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("sbyte[]", typeof(sbyte[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("byte[]", typeof(byte[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("short[]", typeof(short[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ushort[]", typeof(ushort[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("int[]", typeof(int[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("uint[]", typeof(uint[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("long[]", typeof(long[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("ulong[]", typeof(ulong[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("float[]", typeof(float[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("double[]", typeof(double[]));
            ModifyValueConverter.Register<ModifyValue_Parameterless>("decimal[]", typeof(decimal[]));
            #endregion
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

            string caption = LocalizationManager.Current.Interface.Translate("Template_Caption", template.Name, template.Author);

            if (multiField.ShowDialog(texts, caption, tooltips) == true)
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
