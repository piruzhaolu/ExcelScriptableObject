using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace ExcelScriptableObject
{

    /// <summary>
    /// ScriptableObject资源获取类，通过静态方法找到一类配置
    /// </summary>
    public class SOAssets
    {
        public static SOList Find<T>(string path) where T : Object
        {
            return Find(path, typeof(T));
        }


        public static SOList Find(string path, Type type)
        {
            var asset = AssetDatabase.FindAssets("t:" + type.ToString(), new string[] { path });
            var list = new List<Object>();
            foreach (var item in asset)
            {
                string itemPath = AssetDatabase.GUIDToAssetPath(item);
                var itemAsset = AssetDatabase.LoadAssetAtPath(itemPath,type);
                list.Add(itemAsset);
            }
            return new SOList(list, type, path);
        }
    }
}

