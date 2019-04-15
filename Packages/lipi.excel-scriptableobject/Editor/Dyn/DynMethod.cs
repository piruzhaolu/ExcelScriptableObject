using UnityEngine;
using System.Collections;
using System.Reflection;

namespace ExcelScriptableObject
{

    public class DynMethod
    {
        private object _inst;
        private MethodInfo _info;

        public DynMethod(MethodInfo info, object inst)
        {
            if (info == null) return;
            _inst = inst;
            _info = info;
        }
        public DynMethod()
        {
            // 空对象
           
            //_info.Invoke(_inst, )
        }

        public static DynMethod Null
        {
            get => new DynMethod();
        }

        public bool IsNull
        {
            get => _inst == null;
        }

        public object Invoke(params object[] parameters )
        {
            if (IsNull) return null;
            return _info.Invoke(_inst, parameters);

        }


    }

}
