using UnityEngine;
using System.Collections;


namespace ExcelScriptableObject
{
    /// <summary>
    /// Excel导出ScriptObject时如果需要创建资源提供的新资源名称等信息
    /// </summary>
    public class NewAssetInfo
    {
        public string Folder = string.Empty;
        public string FileName = string.Empty;
        public string AssetBundle = string.Empty;

        public string GetPath()
        {
            return $"{Folder}/{FileName}";
        }
    }

}

