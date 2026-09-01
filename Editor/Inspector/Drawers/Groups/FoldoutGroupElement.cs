using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class FoldoutGroupElement : MMHeaderGroupElement
    {
        private readonly string _title;
        private readonly string _key;
        private readonly bool _defaultExpanded;

        public FoldoutGroupElement(MMGroupContext context)
        {
            _defaultExpanded = context.Node.Expanded;
            _title = string.IsNullOrEmpty(context.Node.Title)
                ? MMReflection.ToDisplayName(context.Node.Name)
                : context.Node.Title;

            _key = MMUiState.Key(MMUiState.GroupScope, context.Owner, context.Node.Path);
        }

        protected override bool IsExpanded
        {
            get => MMUiState.GetExpanded(_key, _defaultExpanded);
            set => MMUiState.SetExpanded(_key, value);
        }

        protected override float HeaderHeight => MMGroupHeader.Height;

        protected override RectOffset BodyPadding => MMFrame.BodyPadding;

        protected override float BodyIndent => 0f;

        protected override void DrawBackground(Rect rect)
        {
            DrawFrame(rect);
        }

        protected override void DrawHeader(Rect rect)
        {
            bool expanded = MMGroupHeader.DrawFoldout(rect, IsExpanded, _title);

            if (expanded != IsExpanded)
            {
                IsExpanded = expanded;
            }
        }
    }
}
