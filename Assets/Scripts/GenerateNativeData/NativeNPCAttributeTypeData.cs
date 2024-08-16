using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeNPCAttributeTypeData
{
    public NativeHashMap<int,Int32> effectAmountByStat;
    public Int32 effectAmount;
    public NPCAttributeType attributeType;

    public NativeNPCAttributeTypeData(NPCAttributeTypeData instance)
    {
        effectAmountByStat = new NativeHashMap<int,Int32>(instance.effectAmountByStat.Count, Allocator.Persistent);

        foreach (var element in instance.effectAmountByStat)
            effectAmountByStat.Add((int)element.Key, element.Value);

        effectAmount = instance.effectAmount;

        attributeType = instance.attributeType;

    }

    public void Dispose()
    {
        if (effectAmountByStat.IsCreated)
            effectAmountByStat.Dispose();
    }
}
