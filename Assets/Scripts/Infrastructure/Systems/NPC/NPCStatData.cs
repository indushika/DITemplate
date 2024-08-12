public struct NPCStatData 
{
    private NPCStatType statType;
    private string statName;
    private int description;
    private int minConstraints;
    private int maxConstraints;

    public NPCStatType StatType { get => statType; set => statType = value; }
    public string StatName { get => statName; set => statName = value; }
    public int Description { get => description; set => description = value; }
    public int MinBaseValue { get => minConstraints; set => minConstraints = value; }
    public int MaxBaseValue { get => maxConstraints; set => maxConstraints = value; }
}
