using Cysharp.Threading.Tasks;
using MonsterFactory.Services;
using MonsterFactory.Services.DataManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public interface INPCAssignmentManager
{
    Assignment CreateAndAddAssignment(AssignmentTypeId assignmentType, NPCTaskProvider npcTaskProvider);
    void AssignNPCToAssignment(Assignment assignment, int NPCId);
}
public class NPCAssignmentManager : IMFService, INPCAssignmentManager
{
    private ReadOnlyGameData readOnlyGameData;
    private RuntimeGameData runtimeGameData;
    private readonly MFRuntimeDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider;

    private NPCManager npcManager;
    private NPCTaskRunner npcTaskRunner;

    private Dictionary<AssignmentTypeId, AssignmentTypeData> assignmentTypeDataById;

    private NPCAssigner npcAssigner;
    private Dictionary<int, Assignment> activeAssignmentsById;

    [Inject]
    public NPCAssignmentManager(MFRuntimeDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider, ReadOnlyGameData readOnlyGameData)
    {
        this.runtimeDataInstanceProvider = runtimeDataInstanceProvider;

        this.readOnlyGameData = readOnlyGameData;

        runtimeGameData = runtimeDataInstanceProvider.DataInstance;

        Initialize();
    }

    #region API 
    public UniTask[] GetInitializeTasks()
    {
        return default;
    }

    public Assignment CreateAndAddAssignment(AssignmentTypeId assignmentType, NPCTaskProvider npcTaskProvider)
    {
        AssignmentTypeData assignmentData; 

        if (!assignmentTypeDataById.TryGetValue(assignmentType, out assignmentData))
        {
            if (!readOnlyGameData.AssignmentTypeDataById.TryGetValue(assignmentType, out assignmentData))
            {
                throw new System.Exception("NPCAssignmentManager: AddAssignment: Assignment Type Data not found.");
            }
        }

        int id = InstanceIdProvider.GetInstanceId(activeAssignmentsById.Keys.ToList());

        Assignment assignment = new Assignment(id, assignmentData, npcTaskProvider);

        activeAssignmentsById.Add(id, assignment);

        runtimeGameData.UpdateAssignmentsByIDCollection(activeAssignmentsById);

        npcAssigner.AssignNPCs(assignment);

        npcAssigner.AssignUnassginedNPCsBasedOnPriority(activeAssignmentsById);

        npcTaskRunner.RegisterAndRunTask(npcTaskProvider);

        return assignment;
    }

    public void AssignNPCToAssignment(Assignment assignment, int NPCId)
    {

    }
    #endregion

    #region Implementation
    private void Initialize()
    {
        LoadAssignmentsFromGameData();

        npcAssigner = new NPCAssigner(npcManager.ActiveNPCById);
    }
    private void LoadAssignmentsFromGameData()
    {
        if (readOnlyGameData == null) 
        {
            throw new System.ArgumentNullException("JobsManager: LoadAssignmentsFromGameData: Persistent Game Data not found");
        }

        activeAssignmentsById = runtimeGameData.AssignmentsById;
    }

    #endregion

    #region Event Handlers
    private void AssignmentCompletionEventHandler(Assignment assignment)
    {
        //unassign NPCs 
        //remove assignment from cached data
        //remove assignment from runtime game data
    }

    #endregion

    #region Notes: Remove Later
    //job system should have a manager that always stores all the assignments.
    //Each assignment is different so we need that data too.
    //When building management systems are done the assignment data will have a reference to the instance of the building they got assigned to

    //need a priority system for jobs that just use unassigned npc's. They go between these random tasks without user assigning them
    #endregion
}
