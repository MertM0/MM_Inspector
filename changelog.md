# Changelog

## [0.1.2] - 2026-09-01

- Builds on Unity 6000.0 through 6000.6. `Object.GetInstanceID()` became a compile error in
  6000.5, replaced by the 64 bit `EntityId`.
- Group, tab and button state no longer collides between two nested fields of the same type on
  one object.

## [0.1.1] - 2026-08-31

- `[ProgressBar]` works on `[ShowInInspector]` members. A member without a serialized field is
  drawn read only, so `Editable` has no effect there.
- Groups work inside nested serializable types and list elements. A `[System.Serializable]` class
  can declare its own `[BoxGroup]`, `[TabGroup]` and the rest, which until now were only read off
  the inspected object itself and were silently ignored anywhere deeper. Each instance keeps its
  own foldout and tab state, so sibling fields of one type do not move together.
- Fixed a nested serializable field's foldout arrow landing outside the surrounding group frame.
- The navigation bar is hosted by the inspector window instead of the object header. It stays put
  while the inspector scrolls, sits above the object header, and appears for every selection.
  ScriptableObjects, materials and prefab assets used to get no bar at all.
- Collapsing components with `Ctrl+Shift+E` or `Shift+E` now sticks. The state was written only to
  the editor tracker, which Unity rebuilds on every selection change, so components reopened by
  themselves as soon as you came back to an object.

## [0.1.0] - 2026-08-30

First release.

- Inspector engine with stacking attributes, nested type support and per-type cached schemas
- Groups, conditionals, validation, value drawers, pickers, buttons and decorators
- Optional workflow module: navigation bar, bookmarks, shortcuts, play mode value saving
- Attribute Showcase sample
