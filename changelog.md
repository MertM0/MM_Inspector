# Changelog

## [0.2.3] - 2026-10-02

- The foldout arrow of an array or list whose element type has no MM attributes stays inside a
  box group, tab or other framed group. It was drawn outside the frame's left edge.

## [0.2.2] - 2026-09-30

- The clipboard takes a component's values when you click the copy icon instead of when you paste,
  so a pose copied in the Animation or Timeline preview survives leaving the preview.
- Pasting skips components whose values the animation preview drives unless the Animation window is
  recording, and keeps the copy so it can be pasted again after the preview.

## [0.2.1] - 2026-09-24

- `[AnimatorState]` picks a state of the Animator's controller into a `string` (name) or `int`
  (hash) field. `[AnimatorClip]` picks one of its clips into a `string` or `AnimationClip` field.
- `[AnimatorParam]` also works with an Animator Override Controller.
- Pickers on `string` and object fields always list `None` first, so a picked value can be cleared.
  Animator pickers offer it on `int` fields too and write `0`.
- Play mode saving works on prefab instances. Values and references captured in play mode were
  not found again in edit mode and were silently dropped.
- The play mode save mark is cleared after the values are restored instead of staying on for the
  next play session.
- A picker on an object field shows `Missing: <Type>` for a reference to a deleted asset instead of
  `None`.
- Attributes that read another member (`[Slider]`, `[ProgressBar]`, `[MinMaxSlider]`, `[Dropdown]`
  and the Animator pickers) work on list and array elements. The member was looked up on the
  element instead of on the component.
- Pickers build their list only when its source changes instead of on every layout pass, and play
  mode save and clipboard marks are no longer resolved again on every hierarchy change.
- Custom pickers derived from `MMPickerElement` implement `TryGetSource` and `Collect` instead of
  `TryBuildOptions`.

## [0.2.0] - 2026-09-11

- `[InlineEditor]` draws a referenced object inside the field, `[InlineProperty]` drops a
  serializable type's foldout.
- `[SerializeReference]` fields get a type picker, and the chosen type's members are drawn by the
  engine, attributes included.
- `[Searchable]` puts a search box above a type's fields.
- `[GroupSettings(Sticky = true)]` carries a group over to the fields that follow it; `[EndGroup]`
  ends the run.
- Workflow: component clipboard (copy icon plus a paste button under `Add Component`, switchable in
  the settings) and minimal mode, toggled with `Shift+M`.

## [0.1.4] - 2026-09-11

- `Hide Script Field` now works on every type the inspector draws, not only on types that declare
  MM attributes.
- Play mode saving reworked. The icon marks a component and the values are captured when you leave
  play mode, so edits made after clicking it are kept too. A second click removes the mark.
- Saved values now survive on prefab instances, object references come back instead of turning into
  dead ids, and marks are no longer lost when a script compiles during play mode.

## [0.1.3] - 2026-09-10

- Fixed a `UnityEngine.Object` reference field (component, asset) drawing as an empty foldout
  instead of an object picker whenever the referenced type itself declared any MM attribute
  anywhere in its members. Drag and drop and the picker were both unreachable. Object references
  are now never drawn nested; only embedded `[System.Serializable]` types are.

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
