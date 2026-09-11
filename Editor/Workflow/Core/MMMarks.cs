namespace MM.Inspector.Workflow.Editor
{
    public static class MMMarks
    {
        public static readonly MMObjectMarks PlayMode = new MMObjectMarks(
            "MM_Inspector.Workflow.PlayModeMarks",
            "Objects created at run time cannot be saved; they have no counterpart in edit mode.");

        public static readonly MMObjectMarks Clipboard = new MMObjectMarks(
            "MM_Inspector.Workflow.Clipboard",
            "Components created at run time cannot be copied; they have no counterpart in edit mode.");
    }
}
