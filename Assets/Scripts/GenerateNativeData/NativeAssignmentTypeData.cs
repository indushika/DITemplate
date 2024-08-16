using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeAssignmentTypeData
{
    public NativeHashMap<int,Single> preferredStatWeightsByType;

    public NativeAssignmentTypeData(AssignmentTypeData instance)
    {
        preferredStatWeightsByType = new NativeHashMap<int,Single>(instance.preferredStatWeightsByType.Count, Allocator.Persistent);

        foreach (var element in instance.preferredStatWeightsByType)
            preferredStatWeightsByType.Add((int)element.Key, element.Value);

    }

    public void Dispose()
    {
        if (preferredStatWeightsByType.IsCreated)
            preferredStatWeightsByType.Dispose();
    }
}
