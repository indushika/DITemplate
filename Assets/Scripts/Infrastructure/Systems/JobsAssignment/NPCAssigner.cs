using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCAssigner 
{
    private Dictionary<int, NPCData> npcDataById;

    private List<int> assignedNPCIds = default;
    private List<int> unassignedNPCIds = default;
    private List<int> dedicatedNPCIds = default;

    public NPCAssigner(Dictionary<int, NPCData> npcDataById)
    {
        this.npcDataById = npcDataById;
    }

    #region API
    public void AssignNPCs(Assignment assignment)
    {
        var assignmentData = assignment.RuntimeData;

        int workerCount = assignmentData.MaxWorkerCount;

        var stats = assignment.PreferredStatWeightsByType.Keys;

        var unassignedNPCIdByCommonStatsCount = new Dictionary<int, int>();

        var assignedNPCIdByCommonStatsCount = new Dictionary<int, int>();

        var commonStatCountByNPCId = new Dictionary<int, int>();

        foreach (int npcID in unassignedNPCIds)
        {
            var npcStats = npcDataById[npcID].BaseValueByStat.Keys.ToList();

            var commonStats = npcStats.Intersect(stats);

            unassignedNPCIdByCommonStatsCount.Add(npcID, commonStats.Count());

            commonStatCountByNPCId.Add(npcID, commonStats.Count());
        }

        foreach (var npcID in assignedNPCIds)
        {
            var npcStats = npcDataById[npcID].BaseValueByStat.Keys.ToList();

            var commonStats = npcStats.Intersect(stats);

            assignedNPCIdByCommonStatsCount.Add(npcID, commonStats.Count());

            commonStatCountByNPCId.Add(npcID, commonStats.Count());
        }

        var npcIdsWithHighestStatCount = commonStatCountByNPCId.OrderByDescending(x => x.Value).Take(workerCount).ToList();

        foreach (var id in npcIdsWithHighestStatCount)
        {
            if (unassignedNPCIds.Contains(id.Key))
            {
                unassignedNPCIds.Remove(id.Key);

                dedicatedNPCIds.Add(id.Key);

                assignment.AssignDedicatedNPC(id.Key);
            }
            else if (assignedNPCIds.Contains(id.Key))
            {
                assignedNPCIds.Remove(id.Key);

                dedicatedNPCIds.Add(id.Key);

                assignment.AssignDedicatedNPC(id.Key);
            }
        }
    }

    public void UnassignNPCs(Assignment assignment)
    {

    }

    //instance equal priority needs to split unassigned NPCs
    //later NPC task manager to 

    //building -> map enitity 
    public void AssignUnassginedNPCsBasedOnPriority(Dictionary<int, Assignment> assignmentsById)
    {
        if (unassignedNPCIds.Count <= 0) return;

        var assignmentsOrderedByPriority = assignmentsById.OrderByDescending(x =>
        {
            var data = x.Value.RuntimeData;
            return data.AssignmentPriority;
        }).ToList();

        foreach (var assignmentById in assignmentsOrderedByPriority)
        {
            var assignment = assignmentById.Value;
            var workerDeficit = assignment.RuntimeData.MaxWorkerCount - assignment.GetTotalWorkerCount;

            do
            {
                int npcId = unassignedNPCIds.First();

                assignment.AssignNPC(npcId);

                unassignedNPCIds.Remove(npcId);

                assignedNPCIds.Add(npcId);

                workerDeficit--;
            }
            while (unassignedNPCIds.Count > 0 && workerDeficit > 0);
        }
    }
    #endregion

    #region Implementation

    #endregion
}
