using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTaskRunner 
{
    //DICTIONARY OF ASSSIGNMENT BY TASK PROVIDER
    //npcId by task 
    private Dictionary<int, NPCTask> activeTasksById;
    private event Action Tick;
   
    public NPCTaskRunner()
    {
        activeTasksById = default;
    }

    #region API
    public void RegisterAndRunTask(NPCTaskProvider taskProvider)
    {
        if (taskProvider == null)
        {
            throw new ArgumentNullException("NPCTaskRunner: RunTask: NPC task Provider not found.");
        }

        var activeTask = taskProvider.GetActiveTask();

        RegisterTask(activeTask);

        activeTask.Run();

        activeTask.OnTaskCompleteEvent += OnTaskCompletionCallback;
    }
    public void PauseTask(int taskId)
    {
        if (activeTasksById.TryGetValue(taskId, out NPCTask task))
        {
            task.Pause();
        }
    }
    public void RunTask(int taskId)
    {
        if (activeTasksById.TryGetValue(taskId, out NPCTask task))
        {
            task.Run();
        }
    }
    public void PauseAllTaks()
    {
        foreach (var taskData in activeTasksById)
        {
            taskData.Value.Pause();
        }
    }
    public void RunAllTaks()
    {
        foreach (var taskData in activeTasksById)
        {
            taskData.Value.Run();
        }
    }
    #endregion

    #region Implementation
    private void RegisterTask(NPCTask task)
    {
        activeTasksById.Add(task.TaskId, task);

        Tick += task.Tick;
    }
    private void UnregisterTask(NPCTask task)
    {
        activeTasksById.Remove(task.TaskId);

        Tick -= task.Tick;
    }

    private void OnTaskCompletionCallback(NPCTask task)
    {
        UnregisterTask(task);
    }
    private void TickBufferedTasks()
    {
        Tick?.Invoke();
    }
    #endregion



}
