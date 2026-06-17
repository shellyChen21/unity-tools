using UnityEditor;
using UnityEngine;

namespace YiHsuan.UITools.Editor
{
    /// <summary>
    /// 範例自訂編輯器視窗。
    /// 開啟方式：Unity 上方選單 Tools / Shelly / UI Tools。
    ///
    /// 驗收用途：
    /// - 選單出現、視窗能開、按鈕能按 = 關卡 1「可編譯」+ Editor asmdef 設定正確。
    ///
    /// 之後把你真正的編輯器工具邏輯替換進來即可。
    /// </summary>
    public class UIToolWindow : EditorWindow
    {
        [MenuItem("Tools/Shelly/UI Tools")]
        public static void Open()
        {
            GetWindow<UIToolWindow>("UI Tools");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("UI Tools", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "這是 package 內的範例編輯器視窗。能看到它，就代表 Editor asmdef 設定成功。",
                MessageType.Info);

            EditorGUILayout.Space();

            if (GUILayout.Button("測試按鈕"))
            {
                Debug.Log("[UITools] Editor 視窗按鈕運作正常。");
            }
        }
    }
}
