# Scene Transition (com.shelly.scenetransition)

以 additive 堆疊方式切換場景，搭配可在 inspector 編輯的轉場流程（淡入淡出遮罩）。Unity 6 / 2023.2 以上適用。

## 前置需求

本套件依賴 UniTask（透過 OpenUPM）。請先在你的專案加入 OpenUPM scoped registry：

Project Settings → Package Manager → Scoped Registries → 新增
- Name: package.openupm.com
- URL: https://package.openupm.com
- Scope: com.cysharp.unitask

## 安裝

Package Manager → Add package from git URL：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.scenetransition#scene/v1.0.0
```

開發階段在本機測試，改用 Add package from disk，指向本資料夾的 `package.json`。

## 內容

- `Runtime/Scripts/SceneTransitionBehaviour.cs` — 轉場抽象基底（`TransitionIn` / `TransitionOut`），繼承它可自訂任意轉場
- `Runtime/Scripts/SceneFader.cs` — 內建預設轉場（`CanvasGroup` 淡入淡出）
- `Runtime/Scripts/SceneStack.cs` — additive 場景堆疊（靜態持久）
- `Runtime/Scripts/SceneSteps.cs` — 流程步驟：`TransitionIn` / `TransitionOut` / `PushScene` / `PopScene` / `PopAllScenes` / `Delay`
- `Runtime/Scripts/ScriptableObjects/SceneWorkflowAsset.cs` — 可編輯的轉場流程資產
- `Runtime/Scripts/SceneLoader.cs` — 對外門面（`Run` / `RunAsync`）
- `Editor/SceneWorkflowAssetEditor.cs` — SceneWorkflowAsset 的自訂 inspector

## 使用步驟

### 1. 做一個轉場 prefab

用內建淡入淡出（最簡單）：
- 一個 Canvas（Render Mode：Screen Space - Overlay，**Sort Order 設高**例如 999，確保蓋在最上層）
- 底下放一張全螢幕 `Image`（通常純黑）
- Canvas（或同一物件）加上 `CanvasGroup` 與 `SceneFader` 元件（淡入/淡出時間在 SceneFader 上調）
- 存成 prefab

想自訂轉場（滑動、自訂動畫…）：繼承 `SceneTransitionBehaviour`，實作 `TransitionIn()`（蓋住）與 `TransitionOut()`（揭開）兩個 `UniTask`，把它掛在你的 prefab 上即可。時間、方向等參數放在你自己的子類上。

```csharp
using Cysharp.Threading.Tasks;
using Shelly.SceneTransition;

public class SlideTransition : SceneTransitionBehaviour
{
    public override async UniTask TransitionIn()  { /* 滑入蓋住畫面 */ }
    public override async UniTask TransitionOut() { /* 滑出揭開畫面 */ }
}
```

### 2. 建立轉場流程資產

專案視窗右鍵 → Create → Shelly → Scene Workflow，得到一個 `SceneWorkflowAsset`：

- 把步驟 1 的 prefab 拖進 **Fader Prefab** 欄位
- 按 **Add Step ▾** 加步驟，排成你要的流程，例如疊上 `Level1`：

  ```
  Transition In
  Push Scene      (從下拉選單選 Level1)
  Transition Out
  ```

  > 淡入/淡出時間是設定在轉場 prefab（SceneFader）上，不在步驟裡。

  > Push Scene 的 Scene Name 是下拉選單，只列出 Build Settings 裡已啟用的場景，
  > 所以選到的場景 runtime 一定載得到。要先把場景加進 Build Settings。

  退回上一層：`Transition In → Pop Scene → Transition Out`
  全部卸載回底層：`Transition In → Pop All Scenes → Transition Out`

> 場景名稱要記得加進 Build Settings。底層（boot）場景由 Unity 正常載入、不進堆疊；Push 把場景疊上去並設為 active，Pop 卸載頂層退回上一層。

### 3. 觸發

```csharp
using Shelly.SceneTransition;

[SerializeField] private SceneWorkflowAsset goToLevel1;

void OnClick()
{
    SceneLoader.Run(goToLevel1);                       // fire & forget
    // SceneLoader.Run(goToLevel1, () => { /* 完成 */ });
    // await SceneLoader.RunAsync(goToLevel1);          // 自己等
}
```

`SceneLoader.StackCount` 可查目前堆疊上的場景數，方便判斷能不能 Pop。

## 版本相依說明

本 package 鎖定 Unity 2023.2 以上，TextMeshPro 已內建於 `com.unity.ugui`；async/await 流程依賴 `com.cysharp.unitask`。
