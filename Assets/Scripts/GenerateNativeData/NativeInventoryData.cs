using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeInventoryData
{
    public NativeHashMap<int,Int32> resourceAmountById;

    public NativeInventoryData(InventoryData instance)
    {
        resourceAmountById = new NativeHashMap<int,Int32>(instance.resourceAmountById.Count, Allocator.Persistent);

        foreach (var element in instance.resourceAmountById)
            resourceAmountById.Add((int)element.Key, element.Value);

    }

    public void Dispose()
    {
        if (resourceAmountById.IsCreated)
            resourceAmountById.Dispose();
    }
}
