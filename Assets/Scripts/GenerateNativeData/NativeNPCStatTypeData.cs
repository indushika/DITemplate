using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeNPCStatTypeData
{
    public NPCStatType statType;
    public Int32 description;
    public Int32 minConstraints;
    public Int32 maxConstraints;

    public NativeNPCStatTypeData(NPCStatTypeData instance)
    {
        statType = instance.statType;

        description = instance.description;

        minConstraints = instance.minConstraints;

        maxConstraints = instance.maxConstraints;

    }

    public void Dispose()
    {
    }
}
