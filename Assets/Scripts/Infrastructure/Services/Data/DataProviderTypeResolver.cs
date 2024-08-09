using System;
using System.Security.Cryptography;
using System.Text;

namespace MonsterFactory.Services.DataManagement
{
    public static class DataProviderTypeResolver
    {
        private static SHA256Managed _sha256Managed;

        public static void ResolveTypeInfo(ref string uniqueId, ref bool autoLoad, ref bool autoSave,
            ref string typeCode, ref MFDataObject dataObject)
        {
            uniqueId = dataObject.UniqueId;
            autoLoad = dataObject.AutoFetch;
            autoSave = dataObject.AutoSave;
            if (!string.IsNullOrEmpty(uniqueId))
            {
                GenerateUidFromDataType(uniqueId, out typeCode);
            }
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