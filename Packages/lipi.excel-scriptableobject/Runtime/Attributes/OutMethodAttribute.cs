
using UnityEngine;
using System.Collections;
using System;

namespace ExcelScriptableObject
{

    public class OutMethodAttribute : Attribute
    {
        public string MethodName { get; }
        public OutMethodAttribute(string method)
        {
            MethodName = method;
        }
    }

}
