namespace MM.Inspector.Editor
{
    public readonly struct MMPickerOption
    {
        public string Label { get; }
        public string Name { get; }
        public int Id { get; }

        public MMPickerOption(string label, string name, int id)
        {
            Label = label;
            Name = name;
            Id = id;
        }

        public MMPickerOption(string name, int id)
            : this(name, name, id)
        {
        }
    }
}
