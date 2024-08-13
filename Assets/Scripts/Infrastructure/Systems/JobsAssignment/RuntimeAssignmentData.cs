using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//extends MF Data Type
public class RuntimeAssignmentData
{
    //dictionary stat type by weight 
    //separate data type for job data that can look up things like preferred stat types (for specific buildings and stuff)
    //using a job type ID e
    //job data is a data type of ours that we save to the file using the Data System 
    //it has the assigned and dedicated NPCs, job priority, status etc, max worker count

    //job runtime data 
    //priority player can change 

    private List<int> assignedNPCs;
    private List<int> dedicatedNPCs;
    private AssignmentPriority assignmentPriority;
    private AssignmentStatus assignmentStatus;
    private int maxWorkerCount;
    public List<int> AssignedNPCs { get => assignedNPCs; set => assignedNPCs = value; }
    public List<int> DedicatedNPCs { get => dedicatedNPCs; set => dedicatedNPCs = value; }

    public AssignmentPriority AssignmentPriority { get => assignmentPriority; set => assignmentPriority = value; }
    public AssignmentStatus AssignmentStatus { get => assignmentStatus; set => assignmentStatus = value; }
    public int MaxWorkerCount { get => maxWorkerCount; set => maxWorkerCount = value; }

    public RuntimeAssignmentData() 
    {
        assignedNPCs = default;

        dedicatedNPCs = default;
    }

}

public enum AssignmentPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Urgent = 3,
}

public enum AssignmentStatus
{
    NotStarted = 0,
    InProgress = 1,
    Paused = 2,
    Completed = 3,
    Blocked = 4,
}