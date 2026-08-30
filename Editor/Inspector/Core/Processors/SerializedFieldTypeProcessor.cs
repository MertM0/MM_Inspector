using System;
using System.Collections.Generic;
using System.Reflection;

namespace MM.Inspector.Editor
{
    internal sealed class SerializedFieldTypeProcessor : MMTypeProcessor
    {
        public override int Order => 0;

        public override void Process(Type type, List<MMMemberSchema> members)
        {
            foreach (FieldInfo field in MMReflection.GetAllFields(type))
            {
                if (MMReflection.IsUnitySerialized(field))
                {
                    members.Add(new MMMemberSchema(field, MMMemberKind.SerializedField));
                }
            }
        }
    }
}
