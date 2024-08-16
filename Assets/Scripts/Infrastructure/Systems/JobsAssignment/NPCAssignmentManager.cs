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
    void AddAssignment(AssignmentTypeId assignmentType, NPCTaskProvider npcTaskProvider);
    void AssignNPCToAssignment(Assignment assignment, int npcId);
    void UnassignNPCFromAssignment(Assignment assignment, int npcId);
}
public class NPCAssignmentManager : IMFService, INPCAssignmentManager
{
    private AssignmentReadOnlyData assignmentReadOnlyData;
    private RuntimeGameData runtimeGameData;

    private NPCManager npcManager;
    private NPCTaskRunner npcTaskRunner;

    private Dictionary<AssignmentTypeId, AssignmentTypeData> assignmentTypeDataById;

    private NPCAssigner npcAssigner;
    private Dictionary<int, Assignment> activeAssignmentsById;

    [Inject]
    public NPCAssignmentManager(MFLocallyStoredDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider,
        MFSerializedReadOnlyDataInstanceProvider<AssignmentReadOnlyData> assignmentReadOnlyDataInstanceProvider)
    {
        assignmentReadOnlyData = assignmentReadOnlyDataInstanceProvider.DataInstance;
        runtimeGameData = runtimeDataInstanceProvider.DataInstance;

        Initialize();
    }

    #region API 
    public void AddAssignment(AssignmentTypeId assignmentType, NPCTaskProvider npcTaskProvider)
    {
        AssignmentTypeData assignmentData; 

        if (!assignmentTypeDataById.TryGetValue(assignmentType, out assignmentData))
        {
            if (!assignmentReadOnlyData.AssignmentTypeDataById.TryGetValue(assignmentType, out assignmentData))
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
    }

    public void AssignNPCToAssignment(Assignment assignment, int npcId)
    {

    }
    public void UnassignNPCFromAssignment(Assignment assignment, int  npcId)
    {

    }

    public UniTask[] GetInitializeTasks()
    {
        return default;
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
        if (assignmentReadOnlyData == null) 
        {
            throw new System.ArgumentNullException("JobsManager: LoadAssignmentsFromGameData: Assignment Read-Only Game Data not found");
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
