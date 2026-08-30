using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class FoldoutGroupElement : MMHeaderGroupElement
    {
        private readonly string _title;
        private readonly string _path;
        private readonly int _ownerId;
        private readonly bool _defaultExpanded;

        public FoldoutGroupElement(MMGroupContext context)
        {
            _defaultExpanded = context.Node.Expanded;
            _title = string.IsNullOrEmpty(context.Node.Title)
                ? MMReflection.ToDisplayName(context.Node.Name)
                : context.Node.Title;

            _path = context.Node.Path;
            _ownerId = context.OwnerId;
        }

        protected override bool IsExpanded
        {
            get => MMUiState.GetExpanded(MMUiState.GroupScope, _ownerId, _path, _defaultExpanded);
            set => MMUiState.SetExpanded(MMUiState.GroupScope, _ownerId, _path, value);
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
