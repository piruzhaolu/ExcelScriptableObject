using UnityEngine;
using System.Collections;
using System;

namespace ExcelScriptableObject
{


    public class InMethodAttribute : Attribute
    {
        public string MethodName { get; }
        public InMethodAttribute(string method)
        {
            MethodName = method;
        }
    }

}
