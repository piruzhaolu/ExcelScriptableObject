using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace ExcelScriptableObject
{

    /// <summary>
    /// 资源展示窗口
    /// </summary>
    public class ExcelSOWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/ExcelScriptableObject")]
        private static ExcelSOWindow OpenWindow()
        {
            var window = GetWindow<ExcelSOWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
            return window;
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            var setting = GetOrCreateSetting();
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Setting",  setting}, 
            };
            var paths = setting.AssetPaths;
            for (int i = 0; i < paths.Count; i++)
            {
                if(paths[i].AssetPath != string.Empty)
                    tree.AddAssetAtPath(paths[i].Name, paths[i].AssetPath);

                if (paths[i].FolderPath != string.Empty)
                    tree.AddAllAssetsAtPath(paths[i].Name, paths[i].FolderPath, typeof(ScriptableObject),true);
            }
            tree.EnumerateTree().AddIcons<IGetIcon>(x => x.GetIcon());

            tree.DefaultMenuStyle.IconSize = 34f;
            tree.DefaultMenuStyle.IconPadding = 0f;
            tree.DefaultMenuStyle.SetIconOffset(-5f);
            tree.DefaultMenuStyle.SetOffset(5f);
            tree.Config.DrawSearchToolbar = true;
            return tree;
        }

        private ExcelSOSetting GetOrCreateSetting()
        {
            var path = "Packages/lipi.excel-scriptableobject/Setting.asset";
            var settings = AssetDatabase.LoadAssetAtPath<ExcelSOSetting>(path);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ExcelSOSetting>();
                AssetDatabase.CreateAsset(settings, path);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }


       

    }
}
