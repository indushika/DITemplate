using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Events;
using MonsterFactory.Services;
using MonsterFactory.Services.DataManagement;
using UnityEngine;

namespace MonsterFactory.TaskManagement
{
    
    //#TODO Deprecate this data system
    public class TaskDataManager : IMFService
    {

        private readonly IAsyncPublisher<PlayerTaskBaseEvent> taskEventPublisher;

        public TaskDataManager( IAsyncPublisher<PlayerTaskBaseEvent> taskEventPublisher)
        {
            this.taskEventPublisher = taskEventPublisher;
        }

        #region API

        public TaskState GetTaskProgressState(string id)
        {


            return TaskState.NotStarted;
        }

        public PlayerTaskProgress GetPlayerTaskProgressById(string id)
        {
    
            return null;
        }

        public void UpdatePlayerTaskProgress(string id, PlayerTaskProgress progress)
        {
            bool shouldPublishState = CheckForStateChange(id, progress);
     
            if (shouldPublishState)
            {
                taskEventPublisher.Publish(ResolveStateEvent(progress));
            }
            
        }

        #endregion


        #region Implementation

        private bool CheckForStateChange(string id, PlayerTaskProgress progress)
        {
            return false;
        }

        private PlayerTaskBaseEvent ResolveStateEvent(PlayerTaskProgress progress)
        {
            switch (progress.taskState)
            {
                case TaskState.InProgress:
                    return new PlayerTaskStarted(progress.taskId);
                case TaskState.Completed:
                    return new PlayerTaskCompleted(progress.taskId);
                case TaskState.Failed:
                    return new PlayerTaskFailed(progress.taskId);
                case TaskState.Aborted:
                    return new PlayerTaskAborted(progress.taskId);
                case TaskState.NotStarted:
                default:
                    return null;
            }
        }

        #endregion


        UniTask[] IMFService.GetInitializeTasks()
        {
            throw new NotImplementedException();
        }
    }
}
