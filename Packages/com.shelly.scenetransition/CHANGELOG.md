# Changelog

本檔記錄各版本變更，版本號遵循 [語意化版本 SemVer](https://semver.org/lang/zh-TW/)。

## [1.0.0] - 2026-06-18

### Added
- `SceneTransitionBehaviour` — 轉場抽象基底（`TransitionIn` / `TransitionOut`），可繼承自訂任意轉場
- `SceneFader` — 內建預設轉場：CanvasGroup 淡入淡出（繼承 `SceneTransitionBehaviour`）
- `SceneStack` — additive 場景堆疊（靜態持久，跨流程 push/pop）
- `SceneWorkflowAsset` — 可在 inspector 編輯的轉場流程資產
- `SceneStep` 步驟：`TransitionIn` / `TransitionOut` / `PushScene` / `PopScene` / `PopAllScenes` / `Delay`
- `SceneLoader` — 對外門面 `Run` / `RunAsync` / `StackCount`
- `SceneWorkflowAssetEditor` — 流程步驟的自訂 inspector：色彩卡片（左側色條依類型 + 編號 + 摘要）、可拖曳排序、Add Step 下拉
- `[SceneName]` attribute + drawer — Push Scene 欄位改用 Build Settings 場景下拉選單
