using System.ComponentModel;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;

namespace TestApp.Data;

public class Armor : INotifyPropertyChanging, INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Power { get; set; }
    public string Id { get; set; }
    public string Tier { get; set; }

    [Name("Type")] public string Slot { get; set; }

    [Name("Equippable")] public string Class { get; set; }

    [Name("Mobility (Base)")] public int Mobility { get; set; }

    [Name("Resilience (Base)")] public int Resilience { get; set; }

    [Name("Recovery (Base)")] public int Recovery { get; set; }

    [Name("Discipline (Base)")] public int Discipline { get; set; }

    [Name("Intellect (Base)")] public int Intellect { get; set; }

    [Name("Strength (Base)")] public int Strength { get; set; }

    private Mod _mod = Mod.None;

    [Ignore]
    internal Mod Mod
    {
        get => _mod;

        set
        {
            OnPropertyChanging(Mod.Name);
            for (var i = 0; i < Constants.Stats.Length; i++)
                this[i] -= _mod[i];

            for (var i = 0; i < Constants.Stats.Length; i++)
                this[i] += value[i];

            _mod = value;
            OnPropertyChanged(Mod.Name);
        }
    }

    internal int this[int statIndex] // this could be switched to enums or factored out of item classes
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

        private set
        {
            switch (statIndex)
            {
                case 0:
                    Mobility = value;
                    break;
                case 1:
                    Resilience = value;
                    break;
                case 2:
                    Recovery = value;
                    break;
                case 3:
                    Discipline = value;
                    break;
                case 4:
                    Intellect = value;
                    break;
                case 5:
                    Strength = value;
                    break;
            }
        }
    }

    internal object this[string propertyName]
    {
        get
        {
            var property = GetType().GetProperty(propertyName);

            if (property is null) throw new NullReferenceException();

            if (property.GetIndexParameters().Length == 0)
            {
                var value = property.GetValue(this, null);
                if (value is null)
                    throw new NullReferenceException();
                return value;
            }

            return property.PropertyType.Name;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void OnPropertyChanging([CallerMemberName] string name = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
    }

    public static int GetSlotIndex(string slot)
    {
        return slot switch
        {
            Constants.SlotHelmet => 0,
            Constants.SlotGauntlets => 1,
            Constants.SlotChest => 2,
            Constants.SlotLeg => 3,
            Constants.SlotHunter or Constants.SlotTitan or Constants.SlotWarlock => 4,
            _ => throw new ArgumentOutOfRangeException(slot)
        };
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}