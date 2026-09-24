using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMGlobalId
    {
        private const int NullIdentifier = 0;

        public static string Of(Object target)
        {
            if (target == null)
            {
                return null;
            }

            GlobalObjectId id = GlobalObjectId.GetGlobalObjectIdSlow(target);

            return id.identifierType == NullIdentifier ? null : id.ToString();
        }

        public static Object Resolve(string id, bool fromPlayMode = false)
        {
            if (string.IsNullOrEmpty(id) || !GlobalObjectId.TryParse(id, out GlobalObjectId parsed))
            {
                return null;
            }

            Object resolved = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(parsed);

            return resolved != null || !fromPlayMode ? resolved : MMFlattenedIds.Find(parsed);
        }
    }
}
