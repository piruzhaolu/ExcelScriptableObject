using System;

namespace ExcelScriptableObject
{
    public class ExcelBindAttribute : Attribute
    {
        public string ExcelName;
        public GenerateType GenType;
        public int HeadRow = 1;
        public int StartRow = 2;
        public string Key = "ID";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelName">文件名.表名 格式, 文件名不包含路径和后缀</param>
        /// <param name="genType">excel中每行对应一个so文件还是整表对应一个so</param>
        public ExcelBindAttribute(string excelName, GenerateType genType = GenerateType.Row)
        {
            ExcelName = excelName;
            GenType = genType;
        }
    }

    public enum GenerateType
    {
        Row,
        Sheet
    }
}
