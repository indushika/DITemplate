
[GenerateNativeData]
public class ResourceTypeData 
{
    public string id;
    public string name;
    public string description;

    //value? could be resource type
    private int baseCost;

    public (int, int) gridSize;

    protected ResourceTypeData(string id, string name, string description, int baseCost)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.baseCost = baseCost;
    }

}

public enum ResourceTypeId
{

}