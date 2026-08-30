using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkNavigation
    {
        public static void Jump(int index)
        {
            IReadOnlyList<MMBookmarkItem> items = MMBookmarkView.Items;

            if (index < 0 || index >= items.Count || !items[index].Available)
            {
                return;
            }

            Object target = MMBookmarkResolver.Resolve(items[index].Entry.Id);

            if (target != null)
            {
                Selection.activeObject = target;
            }
        }
    }
}
