# MM Inspector
[![license](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE.md)
[![Unity](https://img.shields.io/badge/Unity-6000.0%2B-blue.svg)](https://unity.com/)
[![version](https://img.shields.io/badge/version-0.1.0-blue.svg)](changelog.md)

_Attribute driven inspector for Unity_

Attributes stack on a single field, work inside nested types and list elements, and can
read their parameters from other members. Adding your own takes two files.

It also ships an optional [workflow module](#workflow) with a navigation bar, bookmarks,
component shortcuts and play mode value saving, in a separate assembly you can delete.

## Installation

Package Manager → **Add package from git URL**:

```
https://github.com/MertM0/MM_Inspector.git
```

Or add it to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mm.inspector": "https://github.com/MertM0/MM_Inspector.git"
  }
}
```

Requires Unity 6000.0 or newer. No dependencies. The package ships a sample scene under
`Samples/Scenes` with one component per attribute category.

Every attribute lives in one namespace:

```csharp
using MM.Inspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    [BoxGroup("Stats")]
    [Slider(0f, 100f)]
    public float health = 80f;
}
```

## Attributes

<table>
<tr>
<td valign="top">

**Groups**
- [BoxGroup](#groups)
- [VerticalGroup](#groups)
- [HorizontalGroup](#groups)
- [FoldoutGroup](#groups)
- [TabGroup](#groups)
- [GroupSettings](#groupsettings)

</td>
<td valign="top">

**Conditionals**
- [ShowIf](#showif-and-hideif)
- [HideIf](#showif-and-hideif)
- [EnableIf](#enableif-and-disableif)
- [DisableIf](#enableif-and-disableif)
- [ReadOnly](#readonly)

</td>
<td valign="top">

**Validation**
- [Required](#required)
- [ValidateInput](#validateinput)
- [MinValue](#minvalue-and-maxvalue)
- [MaxValue](#minvalue-and-maxvalue)

</td>
<td valign="top">

**Members**
- [Button](#button)
- [ShowInInspector](#showininspector)
- [OnValueChanged](#onvaluechanged)
- [PropertyOrder](#propertyorder)

</td>
</tr>
<tr>
<td valign="top">

**Value Drawers**
- [Slider](#slider)
- [MinMaxSlider](#minmaxslider)
- [ProgressBar](#progressbar)
- [Dropdown](#dropdown)
- [ResizableTextArea](#resizabletextarea)
- [CurveRange](#curverange)

</td>
<td valign="top">

**Pickers**
- [Tag](#pickers)
- [Layer](#pickers)
- [Scene](#pickers)
- [SortingLayer](#pickers)
- [AnimatorParam](#pickers)
- [AssetPreview](#pickers)
- [FilePath](#pickers)
- [FolderPath](#pickers)

</td>
<td valign="top">

**Decorators**
- [Title](#title-and-separator)
- [Separator](#title-and-separator)
- [InfoBox](#infobox)

</td>
<td valign="top">

**Labels**
- [LabelText](#labeltext-and-hidelabel)
- [HideLabel](#labeltext-and-hidelabel)

**Debug**
- [ShowDrawerChain](#showdrawerchain)

</td>
</tr>
</table>

A parameter that names a member is written as a plain member name. Where a parameter could
be either a literal or a member, prefix the member name with `$`.

## Groups

Groups are addressed by path. A group is declared once, by any field that uses it; every
other field just names the path it belongs to. A field lands in the deepest path it names.

```csharp
[BoxGroup("Character")]
public string displayName = "Hero";

[TabGroup("Character/Tabs", "Stats")]
public int level = 1;

[FoldoutGroup("Character/Tabs/Stats/Details")]
public int strength = 10;

[HorizontalGroup("Character/Tabs/Stats/Details/Resistances")]
public int fire = 5;

[HorizontalGroup("Character/Tabs/Stats/Details/Resistances")]
public int frost = 3;
```

`BoxGroup` and `VerticalGroup` draw a frame, `HorizontalGroup` puts its children side by
side, `FoldoutGroup` is collapsible and `TabGroup` builds a tab strip with one child group
per tab. A path segment that no field declares stays invisible and only indents.

### GroupSettings

Group settings live on the class, not on every field. `Expanded` is a foldout's initial
state; once a user toggles it, their choice wins for the rest of the session.

```csharp
[GroupSettings("Character", Title = "Character")]
[GroupSettings("Character/Tabs/Stats/Details", Title = "Details", Expanded = true)]
public class Player : MonoBehaviour { }
```

## Conditionals

### ShowIf and HideIf

Hides the field unless the condition holds. Matches a bool member, or a member against a
value. Several conditions on one field all have to pass.

```csharp
public bool showOptional;

[ShowIf(nameof(showOptional))]
public float optionalValue = 1f;

[HideIf(nameof(mode), MovementMode.Flying)]
public float groundFriction = 0.5f;
```

### EnableIf and DisableIf

Greys the field out instead of hiding it.

```csharp
public bool unlocked = true;

[EnableIf(nameof(unlocked))]
public int editable = 5;
```

### ReadOnly

```csharp
[ReadOnly]
public float alwaysReadOnly = 42f;
```

## Validation

### Required

Shows an error box while the reference or string is empty.

```csharp
[Required]
public GameObject target;

[Required("Pick a material before playing.")]
public Material material;
```

### ValidateInput

Shows an error box while the named bool member is false.

```csharp
[ValidateInput(nameof(IsEven), "Wave count must be even.")]
public int waveCount = 4;

private bool IsEven => waveCount % 2 == 0;
```

### MinValue and MaxValue

Clamps the field when it is edited. Both accept a constant or a member name.

```csharp
[MinValue(0f)]
[MaxValue(100f)]
public int percentage = 50;

[MinValue(nameof(floor))]
public float aboveFloor = 25f;
```

## Value Drawers

### Slider

Works on `int` and `float`. Bounds may be constants or member names.

```csharp
[Slider(0f, 10f)]
public float volume = 3f;

[Slider(nameof(lowerBound), nameof(upperBound))]
public float dynamicRange = 25f;
```

### MinMaxSlider

Works on `Vector2` and `Vector2Int`.

```csharp
[MinMaxSlider(0f, 100f)]
public Vector2 spawnDelay = new Vector2(20f, 80f);
```

### ProgressBar

```csharp
[ProgressBar(0f, nameof(magazineSize), Color = MMColor.Orange, Editable = true, Label = "Ammo")]
public int ammo = 12;
```

Pairs with [ShowInInspector](#showininspector) to watch a runtime value. A member that has no
serialized field behind it is drawn read only, so `Editable` has no effect there.

```csharp
[ShowInInspector]
[ProgressBar(0f, nameof(MaxHealth), Color = MMColor.Red)]
public int CurrentHealth { get; private set; }
```

### Dropdown

Takes any `IEnumerable` member. Use `DropdownList<T>` when the labels differ from the
values. A stored value that is no longer in the list shows as `Missing: X` instead of
silently resetting.

```csharp
[Dropdown(nameof(Difficulties))]
public int difficulty = 3;

private DropdownList<int> Difficulties => new DropdownList<int>
{
    { "Easy", 1 },
    { "Normal", 3 },
    { "Hard", 5 }
};
```

### ResizableTextArea

```csharp
[ResizableTextArea]
public string notes;
```

### CurveRange

```csharp
[CurveRange(0f, 0f, 1f, 1f)]
public AnimationCurve fade = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
```

## Pickers

Each picker accepts either the name or the id of what it selects. A stored value that no
longer exists shows as `Missing: X` instead of silently resetting.

```csharp
[Tag] public string singleTag;
[Layer] public int layerById;
[Scene] public string sceneByName;
[SortingLayer] public int sortingLayerById;

public Animator animator;

[AnimatorParam(nameof(animator))]
public string anyParameter;

[AnimatorParam(nameof(animator), AnimatorControllerParameterType.Trigger)]
public string triggerParameter;

[AssetPreview(96)]
public Sprite icon;

[FilePath(Extensions = "json")]
public string configFile;

[FolderPath]
public string outputFolder;
```

## Members

### Button

Works on methods with parameters too; the arguments get their own fields under the button.

```csharp
[Button]
public void ResetScore() => score = 0;

[Button("Add Points")]
public void AddPoints(int amount, bool doubled) { }
```

### ShowInInspector

Draws a non serialized field or a get-only property, read only.

```csharp
[ShowInInspector]
[NonSerialized]
public float runtimeOnly = 3.14f;

[ShowInInspector]
public int DoubledScore => score * 2;
```

### OnValueChanged

Calls the method after an edit, inside an undo group.

```csharp
[OnValueChanged(nameof(Recalculate))]
public float radius = 1f;
```

### PropertyOrder

Lower values are drawn first.

```csharp
[PropertyOrder(-10)]
public string drawnFirst;
```

## Decorators

### Title and Separator

```csharp
[Title("Combat")]
[Separator]
public int damage = 10;
```

### InfoBox

Takes an optional `VisibleIf` member name.

```csharp
[InfoBox("Damage is applied per hit.", InfoBoxType.Warning)]
public int perHitDamage = 10;
```

## Labels

### LabelText and HideLabel

```csharp
[LabelText("Renamed Field")]
public int renamed = 1;

[HideLabel]
public string withoutLabel;
```

## Debug

### ShowDrawerChain

Prints the drawers wrapping a field, outermost first.

```csharp
[ShowDrawerChain]
[Slider(0f, 10f)]
public float inspected = 4f;
```

## Custom Attributes

Two files, no registration. The attribute goes in a runtime assembly, the drawer in an
editor one.

```csharp
using System;
using MM.Inspector;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class HeaderColorAttribute : MMAttribute
{
    public MMColor Color { get; }

    public HeaderColorAttribute(MMColor color) => Color = color;
}
```

```csharp
using MM.Inspector.Editor;
using UnityEditor;
using UnityEngine;

public sealed class HeaderColorDrawer : MMSimpleDrawer<HeaderColorAttribute>
{
    protected override string Validate(MMProperty property, HeaderColorAttribute attribute)
    {
        return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.String);
    }

    protected override void OnGUI(Rect position, MMProperty property, HeaderColorAttribute attribute)
    {
        EditorGUI.DrawRect(position, MMColorPalette.Get(attribute.Color, MMSkin.Accent));
    }
}
```

`MMSimpleDrawer<T>` covers stateless drawing. Derive from `MMAttributeDrawer<T>` and return
your own `MMElement` when the drawer needs state. The other extension points work the same
way and are all found through `TypeCache`: `MMTypeProcessor` adds members to a type's
schema, `MMHideProcessor<T>` and `MMDisableProcessor<T>` contribute to visibility and the
enabled state, `MMValidator<T>` produces validation results and `MMGroupDrawer<T>` backs a
new group kind.

Which field types an attribute accepts is declared in the drawer's `Validate`. A mismatch
is reported once during setup as an error box above the field, never during drawing.

## Workflow

`Editor/Workflow` is an optional module in its own assembly that does not reference the
engine. Delete the folder and the engine keeps working.

- **Navigation bar.** Selection history (`Alt+Left` / `Alt+Right`) and a bookmark strip.
  It sits at the top of the inspector window, above the object header, and stays there while
  the inspector scrolls. Drag objects from Hierarchy or Project onto the strip to bookmark them,
  drag inside it to reorder. Single click selects, double click pings, right click opens the menu.
- **Shortcuts.** `Ctrl+Shift+E` collapse or expand every component, `Shift+E` collapse all
  but the hovered one, `A` toggle the hovered component, `Backspace` remove it,
  `Alt+1..9` jump to a bookmark.
- **Play mode save.** The icon in a component header stores its values during play and
  restores them when you leave play mode.
- **Script field.** Hides the `Script` row; the type icon in the header opens the file.

The settings are located under **Project Settings → MM Inspector → Workflow**. There are many customization options available.

## Limitations

- **Multi-object editing is partial.** Mixed values are drawn as `—` and an untouched field
  is never written, but conditions, validators and `[Button]` evaluate against the first
  selected object.
- **Collections are drawn by Unity.** `[ListDrawerSettings]` and `[TableList]` are not in
  this release. Attributes inside list elements do work.
- **`[Button]` argument values are not serialized** and reset on domain reload.

## Credits

Inspired by [Odin Inspector](https://odininspector.com),
[Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector),
[vInspector 2](https://assetstore.unity.com/packages/package/252297) and
[NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes).

## License

MM Inspector is [MIT licensed](LICENSE.md).
