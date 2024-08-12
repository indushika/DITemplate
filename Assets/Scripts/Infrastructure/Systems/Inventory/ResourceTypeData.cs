using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTypeData 
{
    private string id;
    private string name;
    private string description;

    //value? could be resource type
    private int baseCost;

    private (int, int) gridSize;

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