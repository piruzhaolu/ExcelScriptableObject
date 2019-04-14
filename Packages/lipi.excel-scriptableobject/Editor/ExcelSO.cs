using System;
using UnityEditor;
using UnityEngine;

namespace ExcelScriptableObject
{

    /// <summary>
    /// 进行Excel和ScriptableObject之间同步数据的类
    /// </summary>
    public class ExcelSO
    {

        public static void SOtoExcel(SOList soList, ExcelSheet excelSheet)
        {
            //var map = excelSheet.ToExcelMapper(1, 2, "ID");
            //map["30002", "PrefabName"].Value = "狗日的";
            //map.Save();
            //map.Dispose();

            var bindInfo = soList.BindInfo;
            using(var map = excelSheet.ToExcelMapper(bindInfo.HeadRow, bindInfo.StartRow, bindInfo.Key))
            {
                for (int i = 0; i < soList.List.Count; i++)
                {
                    var DynObj = Dyn.Object(soList.List[i]);
                    string keyValue = DynObj.Q<KeyFieldBindAttribute>().GetValue()?.ToString();

                    if (map.ContainsID(keyValue) == false)
                    {
                        map.AddRow(keyValue);
                    }

                    var members = DynObj.GetDynMembers<FieldBindAttribute>();
                    for (int j = 0; j < members.Count; j++)
                    {
                        var cell = map[keyValue, members[j].Item2.FieldName];
                        if (cell != null)
                        {
                            cell.Value = members[j].Item1.GetValue();
                        }
                        else
                        {
                            Debug.LogError($"excel表中找不到{keyValue}.{members[j].Item2.FieldName}");
                        }
                        
                    }
                }
                map.Save();
            }

        }



        public static void ExcelToSO(ExcelSheet excelSheet, SOList soList)
        {
            var bindInfo = soList.BindInfo;
            var excelList = excelSheet.ToList(bindInfo.HeadRow, bindInfo.StartRow, bindInfo.Key);

            for (int i = 0; i < excelList.Count; i++)
            {
                var excelRow = excelList[i];
                var keyValue = excelRow[bindInfo.Key];
                var exist = false;
                for (int j = 0; j < soList.List.Count; j++)
                {
                    var so = soList.List[j];
                    var DynObj = Dyn.Object(so);
                    var item = DynObj.Q<KeyFieldBindAttribute>().GetValue();
                    
                    if (item != null && ConvertAndEquals(item, keyValue))// item.ToString() == keyValue.ToString())// item.Equals(keyValue))
                    {
                        exist = true;
                        foreach (var rowItem in excelRow)
                        {
                            DynObj.Q<FieldBindAttribute>((attr) => attr.FieldName == rowItem.Key).SetValue(rowItem.Value);
                        }
                    }
                }

                if (!exist)
                { // so中不存在，创建新的
                    var so = ScriptableObject.CreateInstance(soList.ScriptableObjectType);
                    var DynObj = Dyn.Object(so);
                    DynObj.Q<KeyFieldBindAttribute>().SetValue(keyValue);
                    foreach (var rowItem in excelRow)
                    {
                        DynObj.Q<FieldBindAttribute>((attr) => attr.FieldName == rowItem.Key).SetValue(rowItem.Value);
                    }
                    AssetDatabase.CreateAsset(so, $"{soList.Path}/{keyValue}.asset");
                }
            }
        }


        // 先转换两对象的类型再判断是否相等
        private static bool ConvertAndEquals(object hostValue, object convertValue)
        {
            var newVal = Convert.ChangeType(convertValue, hostValue.GetType());
            return hostValue == newVal;
        }

    }
}
