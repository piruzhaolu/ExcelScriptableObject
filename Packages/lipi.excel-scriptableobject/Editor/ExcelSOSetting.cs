using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using Object = UnityEngine.Object;

namespace ExcelScriptableObject
{

    public class ExcelSOSetting : ScriptableObject
    {

        public string ExcelPaths;

        //private string[] _ExcelPaths;


        private List<(ExcelBindAttribute, Type)> attrList;


        [TableList]
        public List<AssetPathItem> AssetPaths;


        [Button("ExcelToSO",ButtonHeight =24), HorizontalGroup("Button")]
        public void ExcelToSO()
        {
            var list = GetAttrItem();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var (pathItem, attr, type) = list[i];
                
                if (pathItem.ExcelName != string.Empty)
                {
                    var excelSheet = ExcelFile.FindSheet(ExcelPaths,pathItem.ExcelName);
                    var assetList = SOAssets.Find(pathItem.FolderPath, type);
                    ExcelSO.ExcelToSO(excelSheet, assetList);
                }
            }

        }

        [Button("SOToExcel", ButtonHeight = 24), HorizontalGroup("Button")]
        public void SOToExcel()
        {
            var list = GetAttrItem();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var (pathItem, attr, type) = list[i];
                if (pathItem.ExcelName != string.Empty)
                {
                    var excelSheet = ExcelFile.FindSheet(ExcelPaths, pathItem.ExcelName);
                    var assetList = SOAssets.Find(pathItem.FolderPath, type);
                    ExcelSO.SOtoExcel(assetList, excelSheet);
                }
                Debug.Log($"{pathItem}, {attr}, {type}");
            }
        }



        private void ExcelListToSOList(List<Dictionary<string, object>> excelList, 
            SOList soList, string key)
        {
            for (int i = 0; i < excelList.Count; i++)
            {
                var excelRow = excelList[i];
                var keyValue = excelRow[key];
                for (int j = 0; j < soList.List.Count; j++)
                {
                    var so = soList.List[j];
                    var DynObj = Dyn.Object(so);
                    var item = DynObj.Q<KeyFieldBindAttribute>((attr) => true).GetValue();
                    if (item != null && item.ToString() == keyValue.ToString())
                    {
                        foreach(var rowItem in excelRow)
                        {
                            DynObj.Q<FieldBindAttribute>((attr) => attr.FieldName == rowItem.Key).SetValue(rowItem.Value);
                        }
                    }
                    
                }

            }
        }


        /// <summary>
        /// 获取每种资源的绑定信息。
        /// AssetPathItem so的路径和excel表名
        /// ExcelBindAttribute Attribute信息 绑定的行和生成方式等
        /// Type so的类型
        /// </summary>
        /// <returns></returns>
        private List<(AssetPathItem,ExcelBindAttribute, Type)> GetAttrItem()
        {
            var list = new List<(AssetPathItem, ExcelBindAttribute, Type)>();
            if (AssetPaths == null) return null;
            if (attrList == null) attrList = Dyn.FindCustomAttribute<ExcelBindAttribute>();
            for (int i = 0; i < AssetPaths.Count; i++)
            {
                var assetPath = AssetPaths[i];
                if (assetPath.Checkbox && assetPath.ExcelName != string.Empty)
                {
                    for (int j = 0; j < attrList.Count; j++)
                    {
                        if (attrList[j].Item1.ExcelName == assetPath.ExcelName)
                        {
                            list.Add((assetPath,attrList[j].Item1, attrList[j].Item2));
                        }
                    }
                }
            }
            return list;
        }
        

    }


    [System.Serializable]
    public class AssetPathItem
    {
        [TableColumnWidth(30, false)][HideLabel][VerticalGroup(" ")]
        public bool Checkbox = false;

        [TableColumnWidth(90, false)]
        public string Name;
        
        [FilePath]
        public string AssetPath;

        [FolderPath]
        public string FolderPath;


        public string ExcelName;
    }

}


//var excelDataList = excelSheet.ToList(attr.HeadRow, attr.StartRow, attr.Key);
//if (attr.GenType == GenerateType.Row)
//{ // 如果按行生成则读取文件夹
//    var assetList = SOAssets.Find(pathItem.FolderPath, type);// 读取文件夹下所有文件
//    ExcelListToSOList(excelDataList, assetList, attr.Key);
//}