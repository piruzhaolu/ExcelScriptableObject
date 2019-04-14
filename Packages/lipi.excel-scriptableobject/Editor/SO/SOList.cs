using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

namespace ExcelScriptableObject
{
    /// <summary>
    /// 某一类型ScriptableObject配置。 
    /// 如存储所有物品配置的文件夹，或是一个等级血量关系的配置
    /// </summary>
    public class SOList
    {
        //private Type _type;
       //private string _path;

        public SOList(List<Object> list, Type type, string path)
        {
            List = list;
            ScriptableObjectType = type;
            Path = path;
            if (type.IsSubclassOf(typeof(ScriptableObject)) == false)
            {
                throw new Exception($"{type} 需要是继承 ScriptableObject类");
            }
            BindInfo = Dyn.GetAttribute<ExcelBindAttribute>(type) ;



        }

        public List<Object> List { get; }
        public ExcelBindAttribute BindInfo { get; }
        public Type ScriptableObjectType { get; }
        public string Path { get; }








    }
}

