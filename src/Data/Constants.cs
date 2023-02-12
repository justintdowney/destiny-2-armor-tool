namespace DestinyArmorTool.Data;

public static class Constants
{
    public static readonly string[] Stats =
    {
        "Mobility", "Resilience", "Recovery", "Discipline", "Intellect", "Strength"
    };

    public const string SlotHelmet = "Helmet";
    public const string SlotGauntlets = "Gauntlets";
    public const string SlotChest = "Chest Armor";
    public const string SlotLeg = "Leg Armor";
    public const string SlotClass = "Class Armor";
    public const string SlotHunter = "Hunter Cloak";
    public const string SlotTitan = "Titan Mark";
    public const string SlotWarlock = "Warlock Bond";

    public const int MaxSlots = 5;


    public const string FragmentPath = "src\\Resources\\FragmentData.csv";
    public const string ModPath = "src\\Resources\\ModData.csv";

    public const string HelpText = "Instructions:\n" +
    "Step 1: Download destinyArmor.csv from Destiny Item Manager (DIM) at dim.gg.\n" +
    "Step 2: Import destinyArmor.csv using the Import option under File on the menu bar.\n" +
    "Step 3: You may select the armor pieces, fragments, and mods to your liking.\n" +
    "Step 4: Export and follow the dialog instructions to transition to using the build in-game.\n";

    public const string AboutText = "Copyright 2023 Justin Downey\n" +
    "Version 1.0\n" +
    "This program is used to produce armor 'builds' in the game Destiny 2.\n";

    public const string Unmarked = "[ ] ";
    public const string Marked = "[X] ";
}