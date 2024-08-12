using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCTask
{
    private int taskId;

    //move these to the task provider
    private int? blockedByTaskId = null;
    private int? blocksTaskId = null;
    private TaskStatus status;
    private bool isRepeating;

    private float progress;

    public int TaskId { get => taskId; }
    public int? BlockedByTaskId { get => blockedByTaskId; }
    public int? BlocksTaskId { get => blocksTaskId; }
    public bool IsRepeating { get => isRepeating; }
    public float Progress { get => progress; set => progress = Mathf.Clamp(value, 0f, 1f); }
    public TaskStatus Status { get => status; }

    //Or use Event Handler
    public event Action<NPCTask> OnTaskCompleteEvent;

    protected NPCTask(int taskId, bool isRepeating)
    {
        this.taskId = taskId;

        this.isRepeating = isRepeating;
    }


    #region API
    public abstract void Tick();
    public virtual void Pause()
    {
        status = TaskStatus.Paused;
    }
    public virtual void Run()
    {
        status = TaskStatus.InProgress;
    }
    public virtual void UpdateTaskInfo(TaskStatus status, int? blockedByTaskId, int? blocksTaskId)
    {
        this.status = status;

        this.blockedByTaskId = blockedByTaskId;

        this.blocksTaskId = blocksTaskId;
    }
    #endregion
}

public enum TaskStatus
{
    NotStarted = 0,
    InProgress = 1,
    Paused = 2,
    Completed = 3,
    Blocked = 4,
}


