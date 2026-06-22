# unity-tools

Shelly 的 Unity 工具庫（mono-repo）。每個工具是一個獨立的 UPM package，可單獨匯入。

## 結構

```
unity-tools/
└── Packages/
    ├── com.shelly.uitools/          ← UI 工具
    ├── com.shelly.scenetransition/  ← 場景堆疊轉場
    └── com.shelly.networking/       ← REST API / 網路
```

| 套件 | 說明 | tag 前綴 |
|---|---|---|
| `com.shelly.uitools` | UI 工具 | `uitools/` |
| `com.shelly.scenetransition` | 場景堆疊轉場 | `scene/` |
| `com.shelly.networking` | REST API / 網路 | `net/` |

## 匯入單一工具

Package Manager → Add package from git URL，用 `?path=` 指到目標工具：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.uitools#uitools/vX.Y.Z
```

- `?path=` 決定只拉哪一個工具，其他不會被帶進使用端專案
- `#uitools/vX.Y.Z` 為 scoped tag，讓每個工具在共用 repo 內保有各自獨立的版本軸
- **版號見各套件 tags**（GitHub 的 tags / releases 頁，或各套件的 CHANGELOG）

## 版本標籤規則（維護備忘）

git tag 是 repo 層級、各工具共用。為了讓每個工具獨立版控，**tag 一律帶工具前綴**：

- UI 工具：`uitools/v1.0.0`、`uitools/v1.1.0` …
- 場景轉場工具：`scene/v1.0.0` …
- 網路工具：`net/v1.0.0` …

如此即使多個工具同在一個 repo，版本號也互不牽連。
