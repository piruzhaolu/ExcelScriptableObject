using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelScriptableObject
{
    public class ExcelFile
    {
        /// <summary>
        /// 返回一个表
        /// </summary>
        /// <param name="directory"></param>
        public static ExcelSheet FindSheet(string directory, string excelName)
        {
            var _directory = directory.Split(';');
            var excel = excelName.Split('#');
            var excelFileName = excel[0];
            var sheetName = excel.Length > 0 ? excel[1] : "Sheet1";
            for (int i = 0; i <= _directory.Length; i++)
            {
                var dir = (i == _directory[i].Length) ? "" : _directory[i];
                var path = dir + excelFileName;
                var fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    return new ExcelSheet(fileInfo, sheetName);
                  
                }
            }
            return null;
        }

        public static ExcelSheet FindSheet(string excelName)
        {
            return FindSheet(string.Empty, excelName);
        }

    }
}
