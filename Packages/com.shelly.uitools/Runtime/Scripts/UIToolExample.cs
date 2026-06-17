using UnityEngine;

namespace YiHsuan.UITools
{
    /// <summary>
    /// 範例 Runtime 元件。掛到場景中任一 GameObject 上，
    /// 進入 Play 模式時印出 log，證明 package 的 Runtime 程式碼已正常載入。
    ///
    /// 驗收用途：
    /// - 看到 log = 關卡 2「可運作」通過。
    /// - 在空專案匯入後仍看到 log = 關卡 3「可攜」通過。
    ///
    /// 之後把你真正的 UI 邏輯替換進來即可。
    /// </summary>
    public class UIToolExample : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("[UITools] Runtime package 已載入並運作。");
        }
    }
}
