namespace DestinyArmorTool.Data;

public class Fragment
{
    public string Subclass { get; set; } = "N/A";
    public string Name { get; set; } = " N/A";
    public int Mobility { get; set; } = 0;
    public int Recovery { get; set; } = 0;
    public int Resilience { get; set; } = 0;
    public int Discipline { get; set; } = 0;
    public int Intellect { get; set; } = 0;
    public int Strength { get; set; } = 0;

    internal int this[int statIndex] => statIndex switch
    {
        0 => Mobility,
        1 => Resilience,
        2 => Recovery,
        3 => Discipline,
        4 => Intellect,
        5 => Strength,
        _ => throw new ArgumentOutOfRangeException(statIndex.ToString())
    };

    public override string ToString()
    {
        var message = $"{Name}".PadRight(25);
        if (Mobility != 0)
            message += $"MOB: {Mobility}".PadRight(10);
        if (Recovery != 0)
            message += $"REC: {Recovery}".PadRight(10);
        if (Resilience != 0)
            message += $"RES: {Resilience}".PadRight(10);
        if (Discipline != 0)
            message += $"DIS: {Discipline}".PadRight(10);
        if (Intellect != 0)
            message += $"INT: {Intellect}".PadRight(10);
        if (Strength != 0)
            message += $"STR: {Strength}".PadRight(10);

        return message;
    }
}