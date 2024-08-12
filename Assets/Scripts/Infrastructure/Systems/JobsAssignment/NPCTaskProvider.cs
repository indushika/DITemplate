using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCTaskProvider 
{
    private Dictionary<int, NPCTask> tasksByOrderOfExecution;
    private NPCTask activeTask;
    private int currentExecutionOrder;
    private float progress;
    //task in progress check Task Sta

    public Dictionary<int, NPCTask> TasksByOrderOfExecution { get => tasksByOrderOfExecution; }
    public float Progress { get => progress; set => progress = Mathf.Clamp(value, 0f, 1f); }

    public event Action<float> TaskExecutionProgressEvent;

    public NPCTaskProvider(Dictionary<int, NPCTask> tasksByOrderOfExecution)
    {
        this.tasksByOrderOfExecution = tasksByOrderOfExecution;

        Initialize();
    }

    #region API
    public NPCTask GetActiveTask()
    {
        return tasksByOrderOfExecution[currentExecutionOrder];
    }
    #endregion

    #region Implementation
    private void Initialize()
    {
        if (tasksByOrderOfExecution == null)
        {
            throw new Exception("NPCTaskProvider: GetActiveTask: Task Collection not found.");
        }
        
        currentExecutionOrder = tasksByOrderOfExecution.First().Key;
    }

    private void UpdateExecutionOrder()
    {
        var index = tasksByOrderOfExecution.Keys.ToList().IndexOf(currentExecutionOrder);
        index++;

        if (tasksByOrderOfExecution.Count() > index)
        {
            currentExecutionOrder = tasksByOrderOfExecution.ElementAt(index).Key;
        }
    }

    private void SubscribeToEvents()
    {
        foreach (var task in tasksByOrderOfExecution)
        {
            task.Value.OnTaskCompleteEvent += OnTaskCompletionCallback;
        }
    }
    private void UnsubscribeToEvents()
    {
        foreach (var task in tasksByOrderOfExecution)
        {
            task.Value.OnTaskCompleteEvent -= OnTaskCompletionCallback;
        }
    }

    private void OnTaskCompletionCallback(NPCTask task)
    {
        UpdateExecutionOrder();

        TaskExecutionProgressEvent?.Invoke(CalculateTaskProgression());
    }

    private float CalculateTaskProgression()
    {
        int totalTasks = tasksByOrderOfExecution.Count;
        float overallTasksProgress = 0;

        foreach (var task in tasksByOrderOfExecution)
        {
            if (task.Value.Status != TaskStatus.NotStarted)
            {
                overallTasksProgress += task.Value.Progress;
            }
        }

        Progress = overallTasksProgress / totalTasks;

        return Progress;
    }
    #endregion

}
