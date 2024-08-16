using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeRuntimeNPCData
{
    public NativeHashMap<int,Int32> runtimeValueByStat;
    public float3 gridPosition;
    public Int32 level;

    public NativeRuntimeNPCData(RuntimeNPCData instance)
    {
        runtimeValueByStat = new NativeHashMap<int,Int32>(instance.runtimeValueByStat.Count, Allocator.Persistent);

        foreach (var element in instance.runtimeValueByStat)
            runtimeValueByStat.Add((int)element.Key, element.Value);

        gridPosition = new float3(instance.gridPosition.x, instance.gridPosition.y, instance.gridPosition.z);

        level = instance.level;

    }

    public void Dispose()
    {
        if (runtimeValueByStat.IsCreated)
            runtimeValueByStat.Dispose();
    }
}
