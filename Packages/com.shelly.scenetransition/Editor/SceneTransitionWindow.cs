using UnityEditor;
using UnityEngine;

namespace Shelly.SceneTransition.Editor
{
    /// <summary>
    /// 場景轉場工具的自訂編輯器視窗。
    /// 開啟方式：Unity 上方選單 Tools / Shelly / Scene Transition。
    ///
    /// 驗收用途：
    /// - 選單出現、視窗能開、按鈕能按 = Editor asmdef 設定正確。
    ///
    /// 之後把你真正的工具邏輯（場景清單、轉場設定等）替換進來即可。
    /// </summary>
    public class SceneTransitionWindow : EditorWindow
    {
        private string _sceneName = "";

        [MenuItem("Tools/Shelly/Scene Transition")]
        public static void Open()
        {
            GetWindow<SceneTransitionWindow>("Scene Transition");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Scene Transition", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "這是 package 內的範例編輯器視窗。能看到它，就代表 Editor asmdef 設定成功。",
                MessageType.Info);

            EditorGUILayout.Space();

            _sceneName = EditorGUILayout.TextField("場景名稱", _sceneName);

            if (GUILayout.Button("測試 Log"))
            {
                Debug.Log($"[SceneTransition] Editor 視窗運作正常，目前輸入場景：{_sceneName}");
            }
        }
    }
}
