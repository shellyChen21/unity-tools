# Scene Transition (com.shelly.scenetransition)

場景切換／載入工具，含轉場動畫。Unity 6 / 2023.2 以上適用。

## 安裝

Package Manager → Add package from git URL：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.scenetransition#scene/v1.0.0
```

開發階段在本機測試，改用 Add package from disk，指向本資料夾的 `package.json`。

## 內容

- `Runtime/Scripts/SceneLoader.cs` — 場景載入器（同步 `Load` 與非同步 `LoadAsync`）
- `Editor/SceneTransitionWindow.cs` — 範例編輯器視窗（選單 Tools / Shelly / Scene Transition）
- `Runtime/Resources/` — 轉場用資源（淡入淡出的 Canvas prefab、圖等）放這裡，**勿引用專案 Assets/ 底下的東西**

## 基本用法

```csharp
using Shelly.SceneTransition;

// 直接切換
SceneLoader.Load("MainMenu");

// 非同步載入（不卡畫面），轉場動畫在 LoadAsync 內插入
StartCoroutine(SceneLoader.LoadAsync("Level1"));
```

## 版本相依說明

本 package 鎖定 Unity 2023.2 以上，TextMeshPro 已內建於 `com.unity.ugui`。
若你的轉場動畫要用 PrimeTween，請依 UI Tools 的 README 設定 OpenUPM scoped registry，
並把 `com.kyrylokuzyk.primetween` 加進本 package.json 的 `dependencies`、
把 `PrimeTween` 加進 Runtime asmdef 的 `references`。**實際用到才加。**
