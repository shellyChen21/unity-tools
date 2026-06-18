using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shelly.SceneTransition
{
    /// <summary>
    /// 場景載入器範例。提供最基本的「載入場景」起點，
    /// 之後把你的轉場動畫（淡入淡出等）邏輯加進來即可。
    ///
    /// 用法：
    ///   SceneLoader.Load("MainMenu");                    // 直接切換
    ///   StartCoroutine(SceneLoader.LoadAsync("Level1")); // 非同步載入（不卡畫面）
    ///
    /// 驗收用途：
    /// - 能成功切換場景且 Console 無紅字 = Runtime 程式碼運作正常。
    /// - 在空專案匯入後仍能運作 = 可攜性通過。
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>同步載入場景（畫面會在載入完成前停住）。</summary>
        public static void Load(string sceneName)
        {
            Debug.Log($"[SceneTransition] 載入場景：{sceneName}");
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 非同步載入場景，回傳可 yield 的 IEnumerator。
        /// 轉場動畫可在這個流程的「載入前」與「載入後」插入。
        /// </summary>
        public static IEnumerator LoadAsync(string sceneName)
        {
            // TODO: 在這裡播放「轉場進場」動畫（例如淡出到黑）

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone)
            {
                // op.progress 可拿來驅動載入進度條（0 ~ 0.9）
                yield return null;
            }

            // TODO: 在這裡播放「轉場退場」動畫（例如從黑淡入）
            Debug.Log($"[SceneTransition] 場景載入完成：{sceneName}");
        }
    }
}
