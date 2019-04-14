using UnityEngine;
using System.Collections;
using System;

public class FieldBindAttribute : Attribute
{
    public string FieldName;
    public bool IsKey = false;

    public FieldBindAttribute(string fieldName)
    {
        FieldName = fieldName;
    }

}
