using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeBuildingTypeData
{
    public NativeHashMap<int,Int32> constructionResourceAmountById;
    public Int32 constructionTime;
    public Int32 npcCapacity;

    public NativeBuildingTypeData(BuildingTypeData instance)
    {
        constructionResourceAmountById = new NativeHashMap<int,Int32>(instance.constructionResourceAmountById.Count, Allocator.Persistent);

        foreach (var element in instance.constructionResourceAmountById)
            constructionResourceAmountById.Add((int)element.Key, element.Value);

        constructionTime = instance.constructionTime;

        npcCapacity = instance.npcCapacity;

    }

    public void Dispose()
    {
        if (constructionResourceAmountById.IsCreated)
            constructionResourceAmountById.Dispose();
    }
}
