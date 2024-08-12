using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableResourceType : ResourceTypeData
{
    public ConsumableResourceType(string id, string name, string description, int baseCost) : 
        base(id, name, description, baseCost)
    {
    }
}
