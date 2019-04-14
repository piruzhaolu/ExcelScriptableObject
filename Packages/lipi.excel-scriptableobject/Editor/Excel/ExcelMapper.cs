using UnityEngine;
using System.Collections;
using System;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;

namespace ExcelScriptableObject
{
    public class ExcelMapper : IDisposable
    {
        private ExcelPackage _ExcelPackage;
        private ExcelWorksheet _ExcelWorksheet;

        private Dictionary<string, int> _colMap;
        private Dictionary<string, int> _rowMap;

        private int maxRow;

        public ExcelMapper(FileInfo fileInfo, string sheetName, int headRow, int startRow, string key)
        {
            _ExcelPackage = new ExcelPackage(fileInfo);
            _ExcelWorksheet = _ExcelPackage.Workbook.Worksheets[sheetName];
            var rows = _ExcelWorksheet.Cells.Rows;
            var cols = _ExcelWorksheet.Cells.Columns;
            maxRow = startRow;

            _colMap = new Dictionary<string, int>();
            int keyColIndex = -1;

            for (int i = 1; i <= cols; i++)
            {
                var value = _ExcelWorksheet.Cells[headRow, i].Value;
                if (value == null || value.ToString() == string.Empty) break;
                if (value.ToString() == key)
                {
                    keyColIndex = i;
                }

                _colMap.Add(value.ToString(), i);
            }
            if (keyColIndex == -1)
            {
                throw new Exception($"定义的key:{key}列在表中没有找到");
            }
            

            _rowMap = new Dictionary<string, int>();
            for (int i = startRow; i < rows; i++)
            {
                maxRow = i;
                var keyValue = _ExcelWorksheet.Cells[i, keyColIndex].Value;
                if (keyValue == null || keyValue.ToString() == string.Empty) break;
                _rowMap.Add(keyValue.ToString(), i);
            }
        }


        public ExcelRange this[string id, string field]
        {
            get
            {
                if (_rowMap.TryGetValue(id, out var id_i))
                {
                    if (_colMap.TryGetValue(field, out var field_i))
                    {
                        return _ExcelWorksheet.Cells[id_i, field_i];
                    }
                }
                return null;
            }
        }

        public bool ContainsID(string id)
        {
            return _rowMap.ContainsKey(id);
        }


        public void AddRow(string id)
        {
            _ExcelWorksheet.InsertRow(maxRow, 1);
            _rowMap.Add(id, maxRow);
            maxRow++;
        }


        public void Save()
        {
            _ExcelPackage.Save();
        }

        public void Dispose()
        {
            _ExcelWorksheet?.Dispose();
            _ExcelPackage?.Dispose();
        }
    }

}
