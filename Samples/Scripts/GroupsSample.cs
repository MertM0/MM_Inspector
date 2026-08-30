using UnityEngine;

namespace MM.Inspector.Samples
{
    [GroupSettings("Character", Title = "Character")]
    [GroupSettings("Character/Tabs/Stats/Details", Title = "Details", Expanded = true)]
    public class GroupsSample : MonoBehaviour
    {
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

        [TabGroup("Character/Tabs", "Skills")]
        public float castSpeed = 1.2f;

        [TabGroup("Character/Tabs", "Skills")]
        public float cooldown = 4f;
        
        [HorizontalGroup("Loadout/Columns")]
        [VerticalGroup("Loadout/Columns/Left")]
        public string primary = "Sword";

        [VerticalGroup("Loadout/Columns/Left")]
        public string secondary = "Dagger";

        [VerticalGroup("Loadout/Columns/Right")]
        public string consumable = "Potion";

        [FoldoutGroup("Debug")]
        public bool verboseLogging;

        [FoldoutGroup("Debug")]
        public int seed = 12345;
    }
}
