namespace MM.Inspector
{
    public abstract class GroupAttribute : MMAttribute
    {
        public string Path { get; }

        public virtual string DeclarationPath => Path;

        public virtual string EffectivePath => Path;

        protected GroupAttribute(string path)
        {
            Path = path;
        }
    }
}
