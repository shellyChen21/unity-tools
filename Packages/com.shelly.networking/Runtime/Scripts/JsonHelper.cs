using UnityEngine;

namespace Shelly.Networking
{
    /// <summary>
    /// JSON 序列化輔助，封裝 Unity 內建的 JsonUtility。
    ///
    /// ⚠️ JsonUtility 的限制（若後端 JSON 觸及以下結構，需改用 Newtonsoft）：
    /// - 不支援 Dictionary
    /// - 不支援「頂層為陣列」的 JSON（需包一層物件，見 FromJsonArray）
    /// - 欄位需為 public 或標 [SerializeField]
    /// </summary>
    public static class JsonHelper
    {
        public static string ToJson<T>(T obj) => JsonUtility.ToJson(obj);

        public static T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);

        /// <summary>
        /// 解析「頂層為陣列」的 JSON（JsonUtility 原生不支援）。
        /// 作法：把 [...] 包進 {"items":[...]} 再解析。
        /// </summary>
        public static T[] FromJsonArray<T>(string json)
        {
            string wrapped = "{\"items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
            return wrapper.items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}
