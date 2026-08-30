using System;
using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Editor
{
    internal static class MMTypeInstances
    {
        public static List<TBase> Of<TBase>() where TBase : class
        {
            List<TBase> instances = new List<TBase>();

            foreach (Type candidate in TypeCache.GetTypesDerivedFrom<TBase>())
            {
                if (candidate.IsAbstract || candidate.GetConstructor(Type.EmptyTypes) == null)
                {
                    continue;
                }

                instances.Add((TBase)Activator.CreateInstance(candidate));
            }

            return instances;
        }
    }
}
