# unity-tools

Shelly 的 Unity 工具庫（mono-repo）。每個工具是一個獨立的 UPM package，可單獨匯入。

## 結構

```
unity-tools/
└── Packages/
    └── com.shelly.uitools/      ← UI 工具
    （之後 upload/download、scene 等工具會以同樣方式加在這裡）
```

## 匯入單一工具

Package Manager → Add package from git URL，用 `?path=` 指到目標工具：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.uitools#uitools/v1.0.0
```

- `?path=` 決定只拉哪一個工具，其他不會被帶進使用端專案
- `#uitools/v1.0.0` 為 scoped tag，讓每個工具在共用 repo 內保有各自獨立的版本軸

## 版本標籤規則（重要）

git tag 是 repo 層級、各工具共用。為了讓每個工具獨立版控，**tag 一律帶工具前綴**：

- UI 工具：`uitools/v1.0.0`、`uitools/v1.1.0` …
- 之後的場景工具：`scene/v1.0.0` …
- 上傳下載工具：`updown/v1.0.0` …

如此即使三個工具同在一個 repo，版本號也互不牽連。
