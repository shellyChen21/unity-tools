# Changelog

本檔記錄各版本變更，版本號遵循 [語意化版本 SemVer](https://semver.org/lang/zh-TW/)。

## [1.1.0] - 2026-06-22

### Added
- `RestfulBuilder.StartRequest()` — 回傳原始回應字串的多載（不做 JSON 反序列化，供呼叫端自行 parse）

## [1.0.0] - 2026-06-22

### Added
- `RestfulBuilder` — REST 呼叫的 fluent builder（GET/POST，UniTask async/await；內含 UnityWebRequest Dispose）
- `RestfulBuilder.SetForm` — multipart/form-data 檔案上傳
- `RestfulRequestScriptableObject` — 把 API endpoint 存成 asset（method / domain / api / timeout / headers）
- `Method` / `Header` — 請求方法與標頭 enum
- `JsonHelper` — JsonUtility 序列化輔助（含頂層裸陣列暫解 `FromJsonArray`）
