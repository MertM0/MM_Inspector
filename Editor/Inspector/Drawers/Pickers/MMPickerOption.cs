using UnityEngine;

namespace MM.Inspector.Editor
{
    public readonly struct MMPickerOption
    {
        public string Label { get; }
        public string Name { get; }
        public int Id { get; }
        public Object Value { get; }

        public MMPickerOption(string label, string name, int id, Object value = null)
        {
            Label = label;
            Name = name;
            Id = id;
            Value = value;
        }

        public MMPickerOption(string name, int id)
            : this(name, name, id)
        {
        }
    }
}
