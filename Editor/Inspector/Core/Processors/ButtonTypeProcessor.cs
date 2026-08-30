using System;
using System.Collections.Generic;
using System.Reflection;

namespace MM.Inspector.Editor
{
    internal sealed class ButtonTypeProcessor : MMTypeProcessor
    {
        public override int Order => 30;

        public override void Process(Type type, List<MMMemberSchema> members)
        {
            foreach (MethodInfo method in MMReflection.GetAllMethods(type))
            {
                if (!method.IsDefined(typeof(ButtonAttribute), true))
                {
                    continue;
                }

                members.Add(new MMMemberSchema(method, MMMemberKind.Method));
            }
        }
    }
}
