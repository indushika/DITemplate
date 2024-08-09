using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;


namespace MonsterFactory.Services.DataManagement
{
    public interface ITypeSerializedDBService
    {
        public IMFSerializedDB LocalDBService();

        public UniTask<T> FetchDataFromDb<T>(string typeCode, CancellationToken cancellationToken) where T : MFData;

        public UniTask<bool> WriteDataToDb<T>(string typeCode, CancellationToken cancellationToken, T dataInstance)
            where T : MFData;
    }

    public class MFLocalDBService : IMFService, ITypeSerializedDBService
    {
        private IMFSerializedDB localDB;


        #region Init

        [Inject]
        public MFLocalDBService()
        {
        }

        private UniTask InitializeDataSystems()
        {
            localDB = new MFSqlDB(DataManagerDirectoryHelper.DataObjectPathForUserId("TestUser"));
            return localDB.Initialize();
        }

        public UniTask[] GetInitializeTasks()
        {
            return new UniTask[]
            {
                InitializeDataSystems()
            };
        }


        public IMFSerializedDB LocalDBService()
        {
            return localDB;
        }

        #endregion

        #region API

        public async UniTask<T> FetchDataFromDb<T>(string typeCode, CancellationToken cancellationToken)
            where T : MFData
        {
            try
            {
                DataChunkMap dataChunkMap = await localDB.GetChunkUniqueDataFromKey(typeCode)
                    .AttachExternalCancellation(cancellationToken);
                if (dataChunkMap != null)
                {
                    return await TryProcessDataChunk<T>(typeCode).AttachExternalCancellation(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"DB Fetch {typeCode} Unknown Error : {e}");
                return null;
            }

            return null;
        }

        public async UniTask<bool> WriteDataToDb<T>(string typeCode, CancellationToken cancellationToken,
            T dataInstance) where T : MFData
        {
            try
            {
                return await localDB.WriteSingleDataChunkToId(typeCode, dataInstance?.SerializeDataToBytes())
                    .AttachExternalCancellation(cancellationToken) > 0;
            }
            catch (OperationCanceledException e)
            {
                Debug.LogWarning($"DB Write {typeCode} Operation Cancelled : {e}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"DB Write {typeCode} Unknown Error : {e}");
                return false;
            }
        }

        #endregion


        #region Implementation

        private async UniTask<T> TryProcessDataChunk<T>(string typeCode) where T : MFData
        {
            DataChunkMap dataChunk = await localDB.GetDataChunkById(typeCode);
            MFData var = dataChunk.ExtractDataObjectOfType<T>();
            if (var is T data)
            {
                return data;
            }

            return null;
        }

        #endregion
    }
}