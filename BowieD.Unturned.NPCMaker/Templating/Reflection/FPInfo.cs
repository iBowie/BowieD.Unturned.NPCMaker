using System.Reflection;

namespace BowieD.Unturned.NPCMaker.Templating.Reflection
{
    public sealed class FPInfo
    {
        private enum EMode
        {
            None = 0,
            Field = 1 << 0,
            Property = 1 << 1,
            Both = Field | Property
        }
        private readonly FieldInfo field;
        private readonly PropertyInfo property;
        private readonly EMode mode;

        public FPInfo(FieldInfo field)
        {
            this.field = field;
            mode = EMode.Field;
        }
        public FPInfo(PropertyInfo property)
        {
            this.property = property;
            mode = EMode.Property;
        }
        public FPInfo(FieldInfo field, PropertyInfo property)
        {
            this.field = field;
            this.property = property;
            mode = EMode.Both;
        }

        public void SetValue(object obj, object value)
        {
            if (mode.HasFlag(EMode.Field))
                field.SetValue(obj, value);

            if (mode.HasFlag(EMode.Property))
                property.SetValue(obj, value);
        }
        public object GetValue(object obj)
        {
            if (mode.HasFlag(EMode.Field))
                return field.GetValue(obj);

            if (mode.HasFlag(EMode.Property))
                return property.GetValue(obj);

            return null;
        }
    }
}
