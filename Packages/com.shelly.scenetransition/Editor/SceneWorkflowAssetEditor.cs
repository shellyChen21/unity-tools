using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Shelly.SceneTransition.Editor
{
    /// <summary>
    /// SceneWorkflowAsset 的自訂 inspector：步驟以色彩卡片呈現
    /// （左側色條依類型區分 + 編號 + 可讀摘要），保留拖曳排序/刪除。
    /// 「Add Step」下拉選要加哪種步驟。取代節點圖編輯器。
    /// </summary>
    [CustomEditor(typeof(SceneWorkflowAsset))]
    public class SceneWorkflowAssetEditor : UnityEditor.Editor
    {
        private static readonly Color Blue   = new(0.30f, 0.55f, 0.90f);
        private static readonly Color Green  = new(0.36f, 0.72f, 0.42f);
        private static readonly Color Orange = new(0.90f, 0.60f, 0.25f);
        private static readonly Color Gray   = new(0.55f, 0.55f, 0.58f);

        // 短型別名（含 Step）→ 顯示名稱 + 色彩
        private static readonly Dictionary<string, (string label, Color color)> Info = new()
        {
            { nameof(TransitionInStep),  ("Transition In",  Blue) },
            { nameof(PushSceneStep),     ("Push Scene",     Green) },
            { nameof(PopSceneStep),      ("Pop Scene",      Orange) },
            { nameof(PopAllScenesStep),  ("Pop All Scenes", Orange) },
            { nameof(DelayStep),         ("Delay",          Gray) },
            { nameof(TransitionOutStep), ("Transition Out", Blue) },
        };

        // Add Step 選單順序
        private static readonly (string label, Type type)[] StepTypes =
        {
            ("Transition In",  typeof(TransitionInStep)),
            ("Push Scene",     typeof(PushSceneStep)),
            ("Pop Scene",      typeof(PopSceneStep)),
            ("Pop All Scenes", typeof(PopAllScenesStep)),
            ("Delay",          typeof(DelayStep)),
            ("Transition Out", typeof(TransitionOutStep)),
        };

        private const float StripWidth = 4f;
        private const float Pad         = 4f;

        private SerializedProperty _faderPrefab;
        private SerializedProperty _steps;
        private ReorderableList    _list;

        private void OnEnable()
        {
            _faderPrefab = serializedObject.FindProperty("faderPrefab");
            _steps       = serializedObject.FindProperty("steps");

            _list = new ReorderableList(serializedObject, _steps, true, true, false, true)
            {
                drawHeaderCallback    = rect => EditorGUI.LabelField(rect, "Steps"),
                elementHeightCallback = ElementHeight,
                drawElementCallback   = DrawElement,
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_faderPrefab);
            EditorGUILayout.Space();

            _list.DoLayoutList();

            if (GUILayout.Button("Add Step  ▾"))
                ShowAddMenu();

            serializedObject.ApplyModifiedProperties();
        }

        private float ElementHeight(int index)
        {
            var element = _steps.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element, GUIContent.none, true) + Pad * 2f;
        }

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            var element        = _steps.GetArrayElementAtIndex(index);
            var (label, color) = Visual(element);

            var row = new Rect(rect.x, rect.y + Pad, rect.width, rect.height - Pad * 2f);

            // 淡色背景 + 左側色條
            EditorGUI.DrawRect(row, new Color(color.r, color.g, color.b, 0.10f));
            EditorGUI.DrawRect(new Rect(row.x, row.y, StripWidth, row.height), color);

            // 內容區（縮排避開色條）
            var content = new Rect(row.x + StripWidth + 6f, row.y, row.width - StripWidth - 10f, row.height);
            var summary = $"{index + 1}.  {label}{Detail(element)}";

            EditorGUI.PropertyField(content, element, new GUIContent(summary), true);
        }

        // 依類型回傳顯示名稱與色彩
        private static (string label, Color color) Visual(SerializedProperty element)
        {
            var shortName = ShortTypeName(element);
            return Info.TryGetValue(shortName, out var info)
                ? info
                : (shortName, Gray);
        }

        // 摘要尾巴：Push 顯示目標場景、Delay 顯示秒數
        private static string Detail(SerializedProperty element)
        {
            switch (ShortTypeName(element))
            {
                case nameof(PushSceneStep):
                    var scene = element.FindPropertyRelative("sceneName")?.stringValue;
                    return string.IsNullOrEmpty(scene) ? "  →  (未選擇)" : $"  →  {scene}";

                case nameof(DelayStep):
                    var seconds = element.FindPropertyRelative("seconds")?.floatValue ?? 0f;
                    return $"  ({seconds:0.##}s)";

                default:
                    return string.Empty;
            }
        }

        private static readonly Dictionary<string, string> ShortNameCache = new();

        private static string ShortTypeName(SerializedProperty element)
        {
            var typeName = element.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(typeName))
                return "Step";

            if (ShortNameCache.TryGetValue(typeName, out var cached))
                return cached;

            var shortName = typeName.Substring(typeName.LastIndexOf(' ') + 1);
            shortName     = shortName.Substring(shortName.LastIndexOf('.') + 1);

            ShortNameCache[typeName] = shortName;
            return shortName;
        }

        private void ShowAddMenu()
        {
            var menu = new GenericMenu();
            foreach (var (label, type) in StepTypes)
            {
                var captured = type;
                menu.AddItem(new GUIContent(label), false, () => AddStep(captured));
            }
            menu.ShowAsContext();
        }

        private void AddStep(Type type)
        {
            var asset = (SceneWorkflowAsset)target;
            Undo.RecordObject(asset, "Add Step");
            asset.Steps.Add((SceneStep)Activator.CreateInstance(type));
            EditorUtility.SetDirty(asset);
            serializedObject.Update();
        }
    }
}
