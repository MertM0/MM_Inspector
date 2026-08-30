using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public abstract class MMAttribute : Attribute
    {
    }
}
