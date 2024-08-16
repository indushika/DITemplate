
[GenerateNativeData]
public struct NPCStatTypeData 
{
    public NPCStatType statType;
    public string statName;
    public int description;
    public int minConstraints;
    public int maxConstraints;

    public NPCStatType StatType { get => statType; set => statType = value; }
    public string StatName { get => statName; set => statName = value; }
    public int Description { get => description; set => description = value; }
    public int MinBaseValue { get => minConstraints; set => minConstraints = value; }
    public int MaxBaseValue { get => maxConstraints; set => maxConstraints = value; }
}
