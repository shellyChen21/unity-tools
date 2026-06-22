# Networking (com.shelly.networking)

網路通訊層：REST API（GET/POST），基於 `UnityWebRequest` + UniTask（async/await）。Unity 6 / 2023.2 以上適用。

## 前置需求

本套件依賴 UniTask（透過 OpenUPM）。請先在你的專案加入 OpenUPM scoped registry：

Project Settings → Package Manager → Scoped Registries → 新增
- Name: package.openupm.com
- URL: https://package.openupm.com
- Scope: com.cysharp.unitask

設定後再安裝本套件，UniTask 會自動一併解析安裝。

## 安裝

Package Manager → Add package from git URL：

```
https://github.com/shellyChen21/unity-tools.git?path=/Packages/com.shelly.networking#net/v1.0.0
```

## 內容

- `Runtime/Scripts/Restful/RestfulBuilder.cs` — REST 呼叫的 fluent builder（GET/POST，UniTask；內含 UnityWebRequest Dispose）
- `Runtime/Scripts/Restful/RestfulRequestScriptableObject.cs` — 把 API endpoint 存成 asset（method / domain / api / timeout / headers）
- `Runtime/Scripts/Restful/Method.cs`、`Header.cs` — 請求方法（GET/POST）與標頭 enum
- `Runtime/Scripts/JsonHelper.cs` — JsonUtility 序列化輔助（含頂層陣列暫解）

## 基本用法

```csharp
using Shelly.Networking;
using Cysharp.Threading.Tasks;

// 1) 用 ScriptableObject 設定好 API，一行打
//    （Create → Shelly → Networking → Restful Request 建立設定 asset）
var res = await new RestfulBuilder().SimpleRequest<LoginRes>(loginApi);

// 2) 手動組裝
var res = await new RestfulBuilder()
    .SetUrl("https://api.example.com/login")
    .SetMethod(Method.POST)
    .AddHeader("Authorization", token)
    .SetBody(loginReq)
    .StartRequest<LoginRes>();
```

- 一個 `RestfulBuilder` 打一次：`StartRequest` 完成後 `UnityWebRequest` 已 Dispose。
- 失敗會丟例外，呼叫端以 `try/catch` 處理；成功以 `JsonUtility` 反序列化為指定型別。

### 檔案上傳（multipart/form-data）

夾檔案用 `SetForm(WWWForm)`，檔案讀取／上傳後刪檔等屬於你的應用邏輯：

```csharp
var uploadRaw = await File.ReadAllBytesAsync(filePath);

var form = new WWWForm();
form.AddBinaryData("form", uploadRaw, fileName);

var data = await new RestfulBuilder()
    .SetRequestSO(uploadApi)                            // 或 .SetUrl(url).SetMethod(Method.POST)
    .AddHeader("Authorization", token)            
    .SetForm(form)
    .StartRequest<S2C_GameResult>();

if (data.status != 200)
    throw new Exception(data.message);
```

- `SetForm` 要在 `SetRequestSO` / `SetMethod` **之後**呼叫。
- 上傳這支 API 的 headers **不要放 ContentType** —— `WWWForm` 會自帶含 boundary 的 `Content-Type`。
- 檔案上傳請用上面的明確串接，不能用 `SimpleRequest`（它不含 form）。

## 限制

JSON 走 Unity 內建 `JsonUtility`，**不支援 Dictionary、頂層裸陣列 `[...]`、欄位需 public**。

- 「物件包具名陣列」`{ "items": [...] }`：開一個含對應欄位的 class 即可，`JsonUtility` 原生支援。
- 「最外層裸陣列」`[...]`：`JsonUtility` 解不了，可用 `JsonHelper.FromJsonArray<T>()` 暫解。
- 若結構更複雜，請改用 `com.unity.nuget.newtonsoft-json` 並調整 `JsonHelper`。
