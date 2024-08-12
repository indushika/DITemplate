using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Assignment 
{
    private int id;
    private RuntimeAssignmentData runtimeData;
    private Dictionary<NPCStatType, float> preferredStatWeightsByType;
    private float progress;
    private NPCTaskProvider taskProvider;

    public int Id { get => id; }
    public RuntimeAssignmentData RuntimeData { get => runtimeData;}
    public Dictionary<NPCStatType, float> PreferredStatWeightsByType { get => preferredStatWeightsByType; }
    public float Progress { get => progress; set => progress = Mathf.Clamp(value, 0f, 1f); }
    public NPCTaskProvider TaskProvider { get => taskProvider;}
    public int GetTotalWorkerCount { get => runtimeData.DedicatedNPCs.Count + runtimeData.AssignedNPCs.Count; }

    public event Action<Assignment> OnAssignmentCompletedEvent;

    public Assignment(int id, AssignmentTypeData assignmentData, NPCTaskProvider taskProvider) 
    {
        this.id = id;

        this.taskProvider = taskProvider;

        preferredStatWeightsByType = assignmentData.PreferredStatWeightsByType;

        Initialize();
    }

    #region API
    public void AssignNPCs(List<int> dedicatedNPCs, List<int> assignedNPCs)
    {
        runtimeData.DedicatedNPCs.AddRange(dedicatedNPCs);

        runtimeData.AssignedNPCs.AddRange(assignedNPCs);
    }
   
    public void AssignNPC(int id)
    {
        runtimeData.AssignedNPCs.Add(id);
    }

    public void AssignDedicatedNPC(int id)
    {
        runtimeData.DedicatedNPCs.Add(id);
    }
    #endregion

    #region Implementation
    private void Initialize()
    {
        runtimeData = new RuntimeAssignmentData();

        SubscribeToEvent();
    }
    private void UpdateAssignmentProgress(float progress)
    {
        Progress = progress;

        if (Progress < 1f)
        {
            return;
        }
        OnAssignmentCompletedEvent?.Invoke(this);
    }
    #endregion

    #region Event Subscriptions/Unsubscriptions
    private void SubscribeToEvent()
    {
        taskProvider.TaskExecutionProgressEvent += UpdateAssignmentProgress;
    }

    private void UnsubscribeToEvent()
    {
        taskProvider.TaskExecutionProgressEvent -= UpdateAssignmentProgress;
    }
    #endregion




}
