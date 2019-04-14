using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace ExcelScriptableObject
{
    public class Dyn
    {
        /// <summary>
        /// 返回当前工程所有类型
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllTypes()
        {
            var allAssembly = AppDomain.CurrentDomain.GetAssemblies();
            var list = new List<Type>();
            for (int i = 0; i < allAssembly.Length; i++)
            {
                Assembly assembly = allAssembly[i];
                if (assembly != null)
                {
                    try
                    {
                        Type[] types = assembly.GetTypes();
                        list.AddRange(types);
                    }catch(ReflectionTypeLoadException ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                    
                }
            }
            return list;
        }

        /// <summary>
        ///  查找当前工程所有 T 的子类型
        /// </summary>
        public static List<Type> FindTypes<T>()
        {
            var list = new List<Type>();
            var types = GetAllTypes();
            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].IsSubclassOf(typeof(T))) {
                    list.Add(types[i]);
                }
            }
            return list;
        }

        /// <summary>
        /// 查找当前工程所有定义了 T Attribute的类型
        /// </summary>
        public static List<(T,Type)> FindCustomAttribute<T>() where T:Attribute
        {
            var types = GetAllTypes();
            var list = new List<(T,Type)>();
            for (int i = 0; i < types.Count; i++)
            {
                var attr = types[i].GetCustomAttribute<T>();
                if (attr != null)
                {
                    list.Add((attr, types[i]));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取type上的某个Attribute的实例值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            var attr = type.GetCustomAttribute<T>();
            return attr;
        }




        public static DynObject Object(object inst)
        {
            return new DynObject(inst);
        }
      
    }

}
