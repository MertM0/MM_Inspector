using System;
using System.Collections.Generic;
using System.Reflection;

namespace MM.Inspector.Editor
{
    internal sealed class ShownPropertyTypeProcessor : MMTypeProcessor
    {
        public override int Order => 20;

        public override void Process(Type type, List<MMMemberSchema> members)
        {
            foreach (PropertyInfo property in MMReflection.GetAllProperties(type))
            {
                if (!property.IsDefined(typeof(ShowInInspectorAttribute), true))
                {
                    continue;
                }

                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                members.Add(new MMMemberSchema(property, MMMemberKind.ShownProperty));
            }
        }
    }
}
