using BowieD.Unturned.NPCMaker.Templating.Reflection;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    public static class ModifyTool
    {
        public static void ApplyModify(Template template, ModifyEntry[] modifyEntries, object result)
        {
            var Modify = modifyEntries;
            var FinalObject = result;

            for (int i = 0; i < Modify.Length; i++)
            {
                ModifyEntry entry = Modify[i];

                if (LogicTool.Evaluate(entry.Expression, entry.Conditions, template))
                {
                    Console.WriteLine($"Applying #{i}");

                    var foType = FinalObject.GetType();

                    FPInfo fp;

                    FieldInfo _fi = foType.GetField(entry.Field);

                    if (_fi == null)
                    {
                        PropertyInfo pi = foType.GetProperty(entry.Field);

                        if (pi == null)
                        {
                            continue;
                        }

                        fp = new FPInfo(pi);
                    }
                    else
                    {
                        fp = new FPInfo(_fi);
                    }

                    var entryValue = entry.Value.GetObject(template);

                    switch (entry.Operation)
                    {
                        case EModifyEntryOperation.set:
                            fp.SetValue(FinalObject, entryValue);
                            break;
                        case EModifyEntryOperation.add:
                            try
                            {
                                var arr = fp.GetValue(FinalObject);
                                var col = arr as IList;
                                col.Add(entryValue);
                            }
                            catch { }
                            break;
                        case EModifyEntryOperation.sum:
                            try
                            {
                                var cur = fp.GetValue(FinalObject);
                                ParameterExpression left = Expression.Parameter(cur.GetType(), "left");
                                ParameterExpression right = Expression.Parameter(entryValue.GetType(), "right");
                                var addMethod = Expression.Lambda<Func<object, object, object>>(Expression.Add(left, right), left, right).Compile();
                                fp.SetValue(FinalObject, addMethod.Invoke(cur, entryValue));
                            }
                            catch { }
                            break;
                        case EModifyEntryOperation.sum2:
                            try
                            {
                                var cur = fp.GetValue(FinalObject);
                                ParameterExpression left = Expression.Parameter(entryValue.GetType(), "left");
                                ParameterExpression right = Expression.Parameter(cur.GetType(), "right");
                                var addMethod = Expression.Lambda<Func<object, object, object>>(Expression.Add(left, right), left, right).Compile();
                                fp.SetValue(FinalObject, addMethod.Invoke(cur, entryValue));
                            }
                            catch { }
                            break;
                        case EModifyEntryOperation.concat:
                            {
                                var cur = fp.GetValue(FinalObject);
                                fp.SetValue(FinalObject, string.Concat(cur, entryValue));
                            }
                            break;
                        case EModifyEntryOperation.concat2:
                            {
                                var cur = fp.GetValue(FinalObject);
                                fp.SetValue(FinalObject, string.Concat(entryValue, cur));
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Skipped modify entry #{i}");
                }
            }
        }
    }
}
