# UI Tools (com.shelly.uitools)

UI 相關工具集。Unity 6 / 2023.2 以上適用。

## 前置需求

本套件依賴 PrimeTween（透過 OpenUPM）。請先在你的專案加入 OpenUPM scoped registry：

Project Settings → Package Manager → Scoped Registries → 新增
- Name: package.openupm.com
- URL: https://package.openupm.com
- Scope: com.kyrylokuzyk.primetween

設定後再安裝本套件，PrimeTween 會自動一併解析安裝。

## 安裝

Package Manager → Add package from git URL：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.uitools#uitools/v1.2.0
```

開發階段在本機測試，改用 Add package from disk，指向本資料夾的 `package.json`。

## 內容

- `Editor/UIToolWindow.cs` — 範例編輯器視窗（選單 Tools / Shelly / UI Tools）
- `Runtime/Resources/` — UI 資源（prefab、圖、字型）放這裡，**勿引用專案 Assets/ 底下的東西**
- `Runtime/Scripts/CanvasResolutionHandler.cs` — Canvas 解析度自適應處理
- `Runtime/Scripts/Utilities/UIVisibility.cs` — CanvasGroup 即時顯示/隱藏
- `Runtime/Scripts/Utilities/UITween.cs` — 常用 Tween 工具（Scale、Alpha）


## 版本相依說明

本 package 鎖定 Unity 2023.2 以上，TextMeshPro 已內建於 `com.unity.ugui`，無需另裝 TMP 套件。
若 Package Manager 對 `com.unity.ugui` 版本報錯，可直接刪除 `package.json` 內 `com.unity.ugui` 那一行（uGUI 為內建套件，本就存在）。