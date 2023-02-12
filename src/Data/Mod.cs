namespace DestinyArmorTool.Data;

public class Mod
{
    public Mod()
    {
        Name = "None";
        Mobility = 0;
        Resilience = 0;
        Recovery = 0;
        Discipline = 0;
        Intellect = 0;
        Strength = 0;
    }

    public string Name { get; set; }
    public int Mobility { get; set; }
    public int Resilience { get; set; }
    public int Recovery { get; set; }
    public int Discipline { get; set; }
    public int Intellect { get; set; }
    public int Strength { get; set; }

    public static readonly Mod None = new ();

    internal int this[int statIndex]
    {
        get
        {
            switch (statIndex)
            {
                case 0: return Mobility;
                case 1: return Resilience;
                case 2: return Recovery;
                case 3: return Discipline;
                case 4: return Intellect;
                case 5: return Strength;
                default: throw new ArgumentOutOfRangeException(statIndex.ToString());
            }
        }
    }

    public override string ToString()
    {
        return Name;
    }
}