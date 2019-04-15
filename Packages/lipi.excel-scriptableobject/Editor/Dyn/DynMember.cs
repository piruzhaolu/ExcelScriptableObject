using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace ExcelScriptableObject
{
    public class DynMember
    {
        private MemberInfo _memberInfo;
        private FieldInfo _fieldInfo;
        private PropertyInfo _propertyInfo;

        private object _inst;

        public static DynMember Null
        {
            get => new DynMember(); 
        }

        public DynMember()
        {
            // 空对象
        }

        //public DynMember(MemberInfo info, object inst)
        //{
        //    _memberInfo = info;
        //    _inst = inst;
        //}

        public DynMember(FieldInfo info, object inst)
        {
            _fieldInfo = info;
            _inst = inst;
        }

        public DynMember(PropertyInfo info, object inst)
        {
            _propertyInfo = info;
            _inst = inst;
        }

        public bool IsNull
        {
            get => _inst == null;
        }

        public string MemberName
        {
            get
            {
                if (IsNull) return string.Empty;

                if (_fieldInfo != null) return _fieldInfo.Name;
                else if (_propertyInfo != null) return _propertyInfo.Name;
                else if (_memberInfo != null) return _memberInfo.Name;
                else return string.Empty;
            }
        }

        public T GetAttribute<T>() where T:Attribute
        {
            if (IsNull) return null;
            if (_fieldInfo != null) return _fieldInfo.GetCustomAttribute<T>();
            else if (_propertyInfo != null) return _propertyInfo.GetCustomAttribute<T>();
            else if (_memberInfo != null) return _memberInfo.GetCustomAttribute<T>();
            else return null;
        }


        public DynMember Q(string name)
        {
            if (IsNull) return this;
            return new DynObject(GetValue()).Q(name);
        }

        public DynMember Q<T>(Func<T, bool> fun) where T : Attribute
        {
            if (IsNull) return this;
            return new DynObject(GetValue()).Q<T>(fun);
        }


        public void SetValue(object value)
        {
            if (IsNull) return;
            if (_fieldInfo != null) _setValue(value, _fieldInfo);
            else if (_propertyInfo != null) _setValue(value, _propertyInfo);
        }

        private void _setValue(object value, FieldInfo info)
        {
            var infoType = info.FieldType;
            object realValue = _getRealValue(value, infoType);
            info.SetValue(_inst, realValue);
        }

        private void _setValue(object value, PropertyInfo info)
        {
            var infoType = info.PropertyType;
            object realValue = _getRealValue(value, infoType);
            info.SetValue(_inst, realValue);
        }

        /// <summary>
        /// 根据infoType 对 value 进行转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        private object _getRealValue(object value,  Type infoType)
        {
            var inMethod = GetAttribute<InMethodAttribute>();
            if (inMethod != null) return Dyn.Object(_inst).Method(inMethod.MethodName).Invoke(value);

            object realValue;
            if (infoType.IsValueType)
            {
                if (value == null)
                {
                    realValue = Activator.CreateInstance(infoType);
                }
                else if (infoType.IsAssignableFrom(value.GetType()))
                {
                    realValue = value;
                }
                else if (value.GetType().IsValueType || value.GetType() == typeof(string))
                {
                    realValue = Convert.ChangeType(value, infoType);
                }
                else
                {
                    realValue = Activator.CreateInstance(infoType);
                }

            }
            else if (infoType == typeof(string))
            {
                if (value == null || value.GetType() == typeof(string))
                {
                    realValue = value;
                }
                else
                {
                    realValue = value.ToString();
                }
            }
            else
            {
                if (value == null || infoType.IsAssignableFrom(value.GetType()))
                {
                    realValue = value;
                }
                else
                {
                    realValue = null;
                }
            }
            return realValue;
        }



        public object GetValue()
        {
            if (IsNull) return null;
            object returnValue = null;
            if (_fieldInfo != null) returnValue = _fieldInfo.GetValue(_inst);
            else if (_propertyInfo != null) returnValue = _propertyInfo.GetValue(_inst);

            var method = GetAttribute<OutMethodAttribute>();
            if (method == null)
            {
                return returnValue;
            } else
            {
                return Dyn.Object(_inst).Method(method.MethodName).Invoke(returnValue);
            }
        }

    }

}
