using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingResourceType : ResourceTypeData
{
    public CraftingResourceType(string id, string name, string description, int baseCost) : 
        base(id, name, description, baseCost)
    {
    }
}
