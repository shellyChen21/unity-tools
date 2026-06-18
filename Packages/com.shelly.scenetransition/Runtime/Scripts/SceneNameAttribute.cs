using UnityEngine;

namespace Shelly.SceneTransition
{
    /// <summary>
    /// 標在 string 欄位上，讓 inspector 改用「拖曳場景資產」的物件欄位來編輯，
    /// 但底層仍存場景名稱字串（runtime 的 SceneManager 需要字串）。
    /// 實際繪製在 Editor 的 SceneNameDrawer。
    /// </summary>
    public class SceneNameAttribute : PropertyAttribute { }
}
