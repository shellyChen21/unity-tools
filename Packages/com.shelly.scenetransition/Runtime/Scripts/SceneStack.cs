using System.Collections.Generic;

namespace Shelly.SceneTransition
{
    /// <summary>
    /// 記錄以 additive 方式疊上去的場景名稱（堆疊）。
    /// 靜態持久，跨流程存活——A 流程 Push 的場景，B 流程才能 Pop。
    /// 不碰 SceneManager，純資料結構。
    /// </summary>
    public static class SceneStack
    {
        private static readonly Stack<string> _stack = new();

        public static int Count => _stack.Count;

        public static void Push(string sceneName) => _stack.Push(sceneName);

        public static string Pop() => _stack.Pop();

        public static string Peek() => _stack.Peek();
    }
}
