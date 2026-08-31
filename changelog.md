# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.1] - 2026-08-31

- `[ProgressBar]` works on `[ShowInInspector]` members. A member without a serialized field is
  drawn read only, so `Editable` has no effect there.
- Groups work inside nested serializable types and list elements. A `[System.Serializable]` class
  can declare its own `[BoxGroup]`, `[TabGroup]` and the rest, which until now were only read off
  the inspected object itself and were silently ignored anywhere deeper. Each instance keeps its
  own foldout and tab state, so sibling fields of one type do not move together.
- Fixed a nested serializable field's foldout arrow landing outside the surrounding group frame.

## [0.1.0] - 2026-08-30

First release.

- Inspector engine with stacking attributes, nested type support and per-type cached schemas
- Groups, conditionals, validation, value drawers, pickers, buttons and decorators
- Optional workflow module: navigation bar, bookmarks, shortcuts, play mode value saving
- Attribute Showcase sample
