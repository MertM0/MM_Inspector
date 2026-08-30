using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMWorkflowShortcuts
    {
        [Shortcut("MM Inspector/History Back", KeyCode.LeftArrow, ShortcutModifiers.Alt)]
        private static void HistoryBack()
        {
            MMSelectionHistory.Back();
        }

        [Shortcut("MM Inspector/History Forward", KeyCode.RightArrow, ShortcutModifiers.Alt)]
        private static void HistoryForward()
        {
            MMSelectionHistory.Forward();
        }

        [Shortcut("MM Inspector/Bookmark 1", KeyCode.Alpha1, ShortcutModifiers.Alt)]
        private static void Bookmark1()
        {
            MMBookmarkNavigation.Jump(0);
        }

        [Shortcut("MM Inspector/Bookmark 2", KeyCode.Alpha2, ShortcutModifiers.Alt)]
        private static void Bookmark2()
        {
            MMBookmarkNavigation.Jump(1);
        }

        [Shortcut("MM Inspector/Bookmark 3", KeyCode.Alpha3, ShortcutModifiers.Alt)]
        private static void Bookmark3()
        {
            MMBookmarkNavigation.Jump(2);
        }

        [Shortcut("MM Inspector/Bookmark 4", KeyCode.Alpha4, ShortcutModifiers.Alt)]
        private static void Bookmark4()
        {
            MMBookmarkNavigation.Jump(3);
        }

        [Shortcut("MM Inspector/Bookmark 5", KeyCode.Alpha5, ShortcutModifiers.Alt)]
        private static void Bookmark5()
        {
            MMBookmarkNavigation.Jump(4);
        }

        [Shortcut("MM Inspector/Bookmark 6", KeyCode.Alpha6, ShortcutModifiers.Alt)]
        private static void Bookmark6()
        {
            MMBookmarkNavigation.Jump(5);
        }

        [Shortcut("MM Inspector/Bookmark 7", KeyCode.Alpha7, ShortcutModifiers.Alt)]
        private static void Bookmark7()
        {
            MMBookmarkNavigation.Jump(6);
        }

        [Shortcut("MM Inspector/Bookmark 8", KeyCode.Alpha8, ShortcutModifiers.Alt)]
        private static void Bookmark8()
        {
            MMBookmarkNavigation.Jump(7);
        }

        [Shortcut("MM Inspector/Bookmark 9", KeyCode.Alpha9, ShortcutModifiers.Alt)]
        private static void Bookmark9()
        {
            MMBookmarkNavigation.Jump(8);
        }

        [Shortcut("MM Inspector/Toggle All Components", KeyCode.E, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
        private static void ToggleAll()
        {
            MMComponentActions.ToggleAllCollapsed();
        }

        [Shortcut("MM Inspector/Collapse Others", KeyCode.E, ShortcutModifiers.Shift)]
        private static void CollapseOthers()
        {
            MMComponentActions.CollapseAllExcept(MMHoverTracker.Hovered);
        }

        [Shortcut("MM Inspector/Toggle Component Enabled", KeyCode.A)]
        private static void ToggleEnabled()
        {
            MMComponentActions.ToggleEnabled(MMHoverTracker.Hovered);
        }

        [Shortcut("MM Inspector/Delete Component", KeyCode.Backspace)]
        private static void DeleteComponent()
        {
            if (MMBookmarkStrip.RemoveHovered())
            {
                return;
            }

            MMComponentActions.Delete(MMHoverTracker.Hovered);
        }
    }
}
