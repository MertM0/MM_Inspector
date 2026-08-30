using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AssetPreviewAttribute : MMAttribute
    {
        private const int DefaultSize = 64;

        public int Size { get; }

        public AssetPreviewAttribute()
        {
            Size = DefaultSize;
        }

        public AssetPreviewAttribute(int size)
        {
            Size = size;
        }
    }
}
