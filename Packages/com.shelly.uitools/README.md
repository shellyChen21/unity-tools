# UI Tools (com.yihsuan.uitools)

UI 相關工具集。Unity 6 / 2023.2 以上適用。

## 安裝

Package Manager → Add package from git URL：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.uitools#uitools/v1.0.0
```

開發階段在本機測試，改用 Add package from disk，指向本資料夾的 `package.json`。

## ⚠️ 第一次的關鍵步驟：產生 .meta 檔

這份骨架**刻意不含 `.meta` 檔**。`.meta` 必須由 Unity 自己產生，才能拿到合法且穩定的 GUID。流程務必照順序：

1. 先用 **Add package from disk** 把本 package 掛進一個 Unity 測試專案
2. Unity 會自動為每個檔案與資料夾產生 `.meta`
3. **確認 Unity 沒報錯後**，回到本資料夾 `git add` 把 `.meta` 一併 commit

> 為什麼這步不能跳過：`.meta` 裡的 GUID 是 prefab、場景引用腳本的依據。若不 commit `.meta`，使用端每次匯入會重新產生不同 GUID，導致引用全部斷裂、prefab 破圖。**.meta 必須跟著 package 進 git。**

## 內容

- `Runtime/Scripts/UIToolExample.cs` — 範例 Runtime 元件
- `Editor/UIToolWindow.cs` — 範例編輯器視窗（選單 Tools / Yi Hsuan / UI Tools）
- `Runtime/Resources/` — UI 資源（prefab、圖、字型）放這裡，**勿引用專案 Assets/ 底下的東西**

## 版本相依說明

本 package 鎖定 Unity 2023.2 以上，TextMeshPro 已內建於 `com.unity.ugui`，無需另裝 TMP 套件。
若 Package Manager 對 `com.unity.ugui` 版本報錯，可直接刪除 `package.json` 內的 `dependencies` 區塊（uGUI 為內建套件，本就存在）。
