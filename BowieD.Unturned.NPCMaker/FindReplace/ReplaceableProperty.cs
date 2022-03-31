using System;
using System.Reflection;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public struct ReplaceableProperty
    {
        private FieldInfo _field;
        private PropertyInfo _property;
        private Func<object, bool> _matcher;

        public string Name { get; }
        public Type TargetType { get; }
        public Type ValueType { get; }
        public FindReplaceFormat ValueFormat { get; }

        public ReplaceableProperty(string name, Type targetType, FindReplaceFormat format, Func<object, bool> matcher = null)
        {
            this.Name = name;
            this.TargetType = targetType;
            this.ValueFormat = format;
            _matcher = matcher;

            PropertyInfo pInfo = targetType.GetProperty(name);

            if (pInfo is null)
            {
                FieldInfo fInfo = targetType.GetField(name);

                if (fInfo is null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    _field = fInfo;
                    _property = null;
                    ValueType = fInfo.FieldType;
                }
            }
            else
            {
                _field = null;
                _property = pInfo;
                ValueType = pInfo.PropertyType;
            }
        }

        public void SetValue(object target, object newValue)
        {
            if (_property is null)
            {
                _field.SetValue(target, newValue);
            }
            else
            {
                _property.SetValue(target, newValue);
            }
        }

        public object GetValue(object target)
        {
            if (_property is null)
            {
                return _field.GetValue(target);
            }
            else
            {
                return _property.GetValue(target);
            }
        }

        public bool CheckValid(object target)
        {
            if (_matcher is null)
                return true;

            return _matcher(target);
        }
    }
}
