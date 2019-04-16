# ExcelScriptableObject
游戏导表工具。处理将Excel数值表导入到程序定义的ScriptableObject,并支持将ScriptableObject中的更改回写到Excel。Excel读写功能依赖EPPlus，Unity显示界面功能依赖odininspector插件

### Excel 到 ScriptableObject


```C#
var excelSheet = ExcelFile.FindSheet("Assets/Excels","excel.xlsx#Sheet1");
var assetList = SOAssets.Find("Assets/So", typeof(MyType));
ExcelSO.ExcelToSO(excelSheet, assetList);
```

### ScriptableObject 到 Excel

```C#
var excelSheet = ExcelFile.FindSheet("Assets/Excels","excel.xlsx#Sheet1");
var assetList = SOAssets.Find("Assets/So", typeof(MyType));
ExcelSO.SOtoExcel(assetList, excelSheet);
```

### ScriptableObject文件的定义
ScriptableObject数据类需要加上Attribute定义来描述导入导出的规则
```C#
[ExcelBind("excel.xlsx#Sheet1",HeadRow = 1,StartRow = 2, Key = "ID")]
public class Config : ScriptableObject
{
    [KeyFieldBind]
    [FieldBind("ID")]
    public int CID;

    [FieldBind("Name")]
    public string Name;
}
```
* ExcelBind 标记当前数据类与哪个表绑定。HeadRow:Excel表中哪行代表标题;StartRow:数据从哪行开始;Key哪列代表索引
* KeyFieldBind 索引列，程序根据这个字段识别每条数据的唯一性
* FieldBind 绑定此字段到表中的哪一列