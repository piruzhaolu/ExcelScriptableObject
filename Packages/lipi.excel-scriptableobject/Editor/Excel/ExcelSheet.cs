using UnityEngine;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace ExcelScriptableObject
{
    public class ExcelSheet
    {
        private FileInfo _fileInfo;
        private string _sheetName;

        public ExcelSheet(FileInfo fileInfo, string sheetName)
        {
            _fileInfo = fileInfo;
            _sheetName = sheetName;
        }

        /// <summary>
        /// 把Excel表转成List, List的项为KeyValue(Dictionary<string,object>)格式
        /// </summary>
        /// <param name="headRow">excel表头所在行（索引从1开始）, 该行数据会转成key的内容</param>
        /// <param name="startRow">excel内容开始行(索引从1开始）</param>
        /// <param name="key">内容的索引列</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ToList(int headRow, int startRow, string key)
        {
            using (var package = new ExcelPackage(_fileInfo))
            {
                var sheet = package.Workbook.Worksheets[_sheetName];
                var rows = sheet.Cells.Rows;
                var cols = sheet.Cells.Columns;
                Dictionary<int, string> head = new Dictionary<int, string>();

                int colsLength = 0;
                int keyIndex = 0; // key列在哪个索引上
                for (int i = 1; i <= cols; i++)
                {
                    var value = sheet.Cells[headRow, i].Value;
                    if (value == null || value.ToString() == string.Empty) break;

                    if ((string) value == key) keyIndex = i;
                    head[i] = (string)value;
                    colsLength++;
                }
                var list = new List<Dictionary<string, object>>();
                for (int i = startRow; i < rows; i++)
                {
                    var keyValue = sheet.Cells[i, keyIndex].Value;
                    if (keyValue == null || keyValue.ToString() == string.Empty) break;
                    var dic = new Dictionary<string, object>();
                    for (int j = 1; j <= colsLength; j++)
                    {
                        dic[head[j]] = sheet.Cells[i, j].Value;
                    }
                    list.Add(dic);
                }
                return list;
            }
        }
        

        public ExcelMapper ToExcelMapper(int headRow, int startRow, string key)
        {
            var map = new ExcelMapper(_fileInfo, _sheetName, headRow, startRow, key);
            return map;
        }






    }

}
