using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public static class FindReplacer
    {
        static FindReplacer()
        {
            Register(new FindReplacerCharacterTargeter());
            Register(new FindReplacerDialogueTargeter());
            Register(new FindReplacerDialogueMessageTargeter());
            Register(new FindReplacerDialogueResponseTargeter());
            Register(new FindReplacerVendorTargeter());
            Register(new FindReplacerVendorItemTargeter());
            Register(new FindReplacerQuestTargeter());
            Register(new FindReplacerConditionTargeter());
            Register(new FindReplacerRewardTargeter());

            RegisterParser<ushort>((str) =>
            {
                if (ushort.TryParse(str, out var parsed))
                {
                    return new Tuple<ushort, bool>(parsed, true);
                }

                return new Tuple<ushort, bool>(0, false);
            });
            RegisterParser<ushort?>((str) =>
            {
                if (ushort.TryParse(str, out var parsed))
                {
                    return new Tuple<ushort?, bool>(parsed, true);
                }

                return new Tuple<ushort?, bool>(null, true);
            });
            RegisterParser<Guid>((str) =>
            {
                if (Guid.TryParse(str, out var parsed))
                {
                    return new Tuple<Guid, bool>(parsed, true);
                }

                return new Tuple<Guid, bool>(Guid.Empty, false);
            });
            RegisterParser<string>((str) => new Tuple<string, bool>(str, true));
        }

        private static readonly List<IFindReplacerTargeter> _targeters = new List<IFindReplacerTargeter>();
        private static readonly Dictionary<Type, Func<string, Tuple<object, bool>>> _parsers = new Dictionary<Type, Func<string, Tuple<object, bool>>>();

        public static IEnumerable<FindReplaceTarget> GetAllTargets(FindReplaceFormat format)
        {
            foreach (var targeter in _targeters)
            {
                foreach (var rp in targeter.ReplaceableProperties)
                {
                    if (rp.ValueFormat.FormatID == format.FormatID)
                    {
                        foreach (var target in targeter.GetTargets())
                        {
                            if (rp.TargetType.IsAssignableFrom(target.GetType()) && rp.CheckValid(target))
                            {
                                yield return new FindReplaceTarget(target, rp, targeter);
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<FindReplaceTarget> FindMatchingTargets(IEnumerable<FindReplaceTarget> targets, object value)
        {
            foreach (var target in targets)
            {
                var targetValue = target.ReplaceableProperty.GetValue(target.Target);

                if (targetValue is IComparable comparable)
                {
                    if (comparable.CompareTo(value) == 0)
                    {
                        yield return target;
                    }
                }
                else
                {
                    if (value == targetValue)
                    {
                        yield return target;
                    }
                }
            }
        }

        public static void ReplaceInAllTargets(IEnumerable<FindReplaceTarget> targets, object newValue)
        {
            foreach (var target in targets)
            {
                target.ReplaceableProperty.SetValue(target.Target, newValue);
            }
        }

        public static void Register<T>(T instance) where T : IFindReplacerTargeter
        {
            _targeters.Add(instance);
        }

        public static void RegisterParser<T>(Func<string, Tuple<T, bool>> parseFunc)
        {
            var t = typeof(T);

            _parsers[t] = (str) =>
            {
                var result = parseFunc(str);

                return new Tuple<object, bool>(result.Item1, result.Item2);
            };
        }

        public static IEnumerable<FindReplaceFormat> GetAllFormats()
        {
            return _targeters
                .SelectMany(d => d.ReplaceableProperties.Select(k => k.ValueFormat))
                .Distinct();
        }

        public static bool TryParse(Type type, string value, out object parsed)
        {
            if (_parsers.TryGetValue(type, out var parser))
            {
                var result = parser.Invoke(value);

                parsed = result.Item1;

                return result.Item2;
            }

            parsed = value;
            return false;
        }
    }
}
