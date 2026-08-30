using System;
using System.Collections.Generic;
using System.Reflection;

namespace MM.Inspector.Editor
{
    internal sealed class ShownFieldTypeProcessor : MMTypeProcessor
    {
        public override int Order => 10;

        public override void Process(Type type, List<MMMemberSchema> members)
        {
            foreach (FieldInfo field in MMReflection.GetAllFields(type))
            {
                if (!field.IsDefined(typeof(ShowInInspectorAttribute), true))
                {
                    continue;
                }

                if (MMReflection.IsUnitySerialized(field))
                {
                    continue;
                }

                members.Add(new MMMemberSchema(field, MMMemberKind.ShownField));
            }
        }
    }
}
