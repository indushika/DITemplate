using System;
using System.Security.Cryptography;
using System.Text;

namespace MonsterFactory.Services.DataManagement
{
    public static class DataProviderTypeResolver
    {
        private static SHA256Managed _sha256Managed;

        /// <summary>
        /// Resolve the MFDataObject attribute related data
        /// Sets the save and load flags if attribute is found.
        /// Generates a UID
        /// </summary>
        /// <param name="autoLoad"> ref AutoLoad : Sets Autoload flag</param>
        /// <param name="autoSave">ref AutoSave : Sets Autosave flag</param>
        /// <param name="typeCode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MFDataObject ResolveTypeInfo<T>(ref string typeCode,ref bool autoLoad, ref bool autoSave)
        {
            MFDataObject dataObject = MFDataSerializerExtensions.GetDataAttribute<T>(out string name);
            string uid = null;
            if (dataObject != null)
            {
                uid = dataObject.UniqueId;
                autoLoad = dataObject.AutoFetch;
                autoSave = dataObject.AutoSave;
            }
            GenerateUidFromDataType( string.IsNullOrEmpty(uid)? name : uid, out typeCode);
            return dataObject;
        }
        
        public static MFDataObject ResolveTypeInfo<T>(ref string typeCode, ref bool autoLoad)
        {
            bool autoSave = default; 
            return ResolveTypeInfo<T>(ref typeCode, ref autoLoad, ref autoSave);
        }

        private static void GenerateUidFromDataType(string uniqueId, out string typeCode)
        {
            _sha256Managed ??= new SHA256Managed();
            byte[] hashedBytes = _sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(uniqueId));
            string hashedString = Convert.ToBase64String(hashedBytes);
            typeCode = hashedString.Length > 64 ? hashedString.Substring(0, 64) : hashedString;
        }
    }
}