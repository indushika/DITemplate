using Cysharp.Threading.Tasks;
using MonsterFactory.Services;
using MonsterFactory.Services.DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public interface INPCManager
{
    NPCData CreateNewNPC();
    NPCData? GetNPCDataById(int id);
    List<int> GetNPCIdsByStatType(NPCStatType statType);
}
public class NPCManager : IMFService, INPCManager
{
    private NPCReadOnlyData npcReadOnlyData;
    private RuntimeGameData runtimeGameData;

    private Dictionary<int, NPCData> activeNPCById;

    private NPCIdProvider idProvider;
    private NPCGenerator generator;

    [Inject]
    public NPCManager(MFLocallyStoredDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider, 
        MFSerializedReadOnlyDataInstanceProvider<NPCReadOnlyData> npcReadOnlyDataInstanceProvider)
    {
        npcReadOnlyData = npcReadOnlyDataInstanceProvider.DataInstance;

        runtimeGameData = runtimeDataInstanceProvider.DataInstance;

        idProvider = new NPCIdProvider(runtimeGameData.NPCIds);

        generator = new NPCGenerator(npcReadOnlyData);

        activeNPCById = new Dictionary<int, NPCData>();
    }

    public Dictionary<int, NPCData> ActiveNPCById { get => activeNPCById;}

    #region API
    public NPCData CreateNewNPC()
    {
        int npcId = idProvider.GetNPCId();

        var npc = generator.GenerateNPCData(npcId, null);

        ActiveNPCById.Add(npcId, npc);

        runtimeGameData.AddNewNPCId(npcId);
        runtimeGameData.AddNewNPC(npcId, npc.RuntimeData);

        return npc;
    }
    public NPCData? GetNPCDataById(int id)
    {
        if (ActiveNPCById == null) 
        {
            throw new NullReferenceException("NPCManager: GetNPCDataById: Active NPC By Id collection is not found");
        }

        if (ActiveNPCById.TryGetValue(id, out NPCData npc))
        {
            return npc;
        }

        return null;
    }

    public List<int> GetNPCIdsByStatType(NPCStatType statType)
    {
        var npcIds = new List<int>();

        foreach (var item in activeNPCById)
        {
            var npcData = item.Value;

            if (npcData.BaseValueByStat.Keys.ToList().Contains(statType))
            {
                npcIds.Add(item.Key);
            }
        }

        return npcIds;
    }

    public UniTask[] GetInitializeTasks()
    {
        return default;
    }
    #endregion

    #region Implementation 
    private void LoadNPCData()
    {
        if (npcReadOnlyData == null)
        {
            throw new NullReferenceException("NPCManager: LoadNPCData: Persistent Game Data not found.");
        }
        var runtimeNPCDataById = runtimeGameData.RuntimeNPCDataById;
        foreach (var data in runtimeNPCDataById)
        {
            var npc = generator.GenerateNPCData(data.Key, data.Value);

            activeNPCById.Add(data.Key, npc);
        }

    }

    #endregion

    #region Notes: Remove Later
    //initial - loads existing data and generate data using generator 

    //NPC creator 
    //generate unique NPC Ids everytime a new NPC is created
    //check against existing ones 
    //generate runtime

    //NPC generator; generates data 

    //fetch existing/ previously generated NPC data using NPC Ids (seed) 
    #endregion

}
