using System;
using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMWorkflowTypes
    {
        public static List<T> Sorted<T>(Func<T, int> order) where T : class
        {
            List<T> instances = new List<T>();

            foreach (Type candidate in TypeCache.GetTypesDerivedFrom<T>())
            {
                if (candidate.IsAbstract || candidate.GetConstructor(Type.EmptyTypes) == null)
                {
                    continue;
                }

                instances.Add((T)Activator.CreateInstance(candidate));
            }

            instances.Sort((first, second) => order(first).CompareTo(order(second)));

            return instances;
        }
    }
}
