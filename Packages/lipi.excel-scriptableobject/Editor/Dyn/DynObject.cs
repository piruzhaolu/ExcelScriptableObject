using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace ExcelScriptableObject
{


    public class DynObject
    {
        private object _inst;


        public DynObject(object inst)
        {
            _inst = inst;
        }

        public bool IsNull
        {
            get => _inst == null;
        }


        public object this[string name]
        {
            get {
                return Q(name).GetValue();
            }
            set {

                Q(name).SetValue(value);
            }
        }

        public List<(DynMember,T)> GetDynMembers<T>() where T:Attribute
        {
            var list = new List<(DynMember,T)>(); 
            if (IsNull) return list;

            var ms = _inst.GetType().GetMembers();
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                var attrs = m.GetCustomAttributes();
                foreach (var item in attrs)
                {
                    if (item.GetType() == typeof(T))
                    {
                        if (m.MemberType == MemberTypes.Field)
                        {
                            list.Add((new DynMember((FieldInfo)m, _inst), (T) item));
                        }
                        else if (m.MemberType == MemberTypes.Property)
                        {
                            list.Add((new DynMember((PropertyInfo)m, _inst), (T)item));
                        }
                    }
                }
            }
            return list;
        }




        public DynMethod Method(string name)
        {
            if (IsNull) return DynMethod.Null;
            var m = _inst.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            return new DynMethod(m,_inst);
        }



        public DynMember Q(string name)
        {
            if (IsNull) return DynMember.Null;
            var field = _inst.GetType().GetField(name);
            if (field == null)
            {
                var property = _inst.GetType().GetProperty(name);
                if (property == null) return DynMember.Null;
                return new DynMember(property, _inst);
            }
            return new DynMember(field, _inst);
        }

        public DynMember Q<T>() where T : Attribute
        {
            return Q<T>(null);
        }


        public DynMember Q<T>(Func<T, bool> fun) where T:Attribute
        {
            if (IsNull) return DynMember.Null;
            var ms = _inst.GetType().GetMembers();
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                var attrs = m.GetCustomAttributes();
                foreach (var item in attrs)
                {
                    if (item.GetType() == typeof(T) && (fun == null || fun((T) item)))
                    {
                        if (m.MemberType == MemberTypes.Field)
                        {
                            return new DynMember((FieldInfo)m, _inst);
                        }else if(m.MemberType == MemberTypes.Property)
                        {
                            return new DynMember((PropertyInfo)m, _inst);
                        }
                    }
                }
            }
            return DynMember.Null;
        }

    }

}
