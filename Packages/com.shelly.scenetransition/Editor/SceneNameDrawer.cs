using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Shelly.SceneTransition.Editor
{
    /// <summary>
    /// 把標了 [SceneName] 的 string 欄位畫成下拉選單，
    /// 選項來自 Build Settings 裡「已啟用」的場景，底層存場景名稱。
    /// 這樣只會選到真的在 build 裡的場景，runtime 才載得到。
    /// </summary>
    [CustomPropertyDrawer(typeof(SceneNameAttribute))]
    public class SceneNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            var buildNames = GetEnabledBuildSceneNames();
            if (buildNames.Count == 0)
            {
                EditorGUI.LabelField(position, label.text, "Build Settings 沒有啟用的場景");
                EditorGUI.EndProperty();
                return;
            }

            var current  = property.stringValue;
            var options  = new List<string>(buildNames);
            var selected = buildNames.IndexOf(current);
            var missing  = selected < 0;

            // 目前值不在 build list（沒加進去或被改名）→ 置頂顯示提示項
            if (missing)
            {
                options.Insert(0, string.IsNullOrEmpty(current)
                    ? "(未選擇)"
                    : $"⚠ {current} (不在 Build Settings)");
                selected = 0;
            }

            EditorGUI.BeginChangeCheck();
            var picked = EditorGUI.Popup(position, label.text, selected, options.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (missing)
                {
                    if (picked > 0) // 0 是提示項，不算選擇
                        property.stringValue = buildNames[picked - 1];
                }
                else
                {
                    property.stringValue = buildNames[picked];
                }
            }

            EditorGUI.EndProperty();
        }

        private static List<string> GetEnabledBuildSceneNames()
        {
            var names = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                    names.Add(Path.GetFileNameWithoutExtension(scene.path));
            }
            return names;
        }
    }
}
