using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Shelly.Networking
{
    /// <summary>
    /// REST API 呼叫的 fluent builder，封裝 UnityWebRequest + UniTask。
    /// 注意：一個 builder 打一次（StartRequest 後 UnityWebRequest 已釋放）。
    /// </summary>
    public class RestfulBuilder
    {
        private readonly UnityWebRequest req;

        public RestfulBuilder()
        {
            req                 = new UnityWebRequest();
            req.downloadHandler = new DownloadHandlerBuffer();
        }

        public RestfulBuilder SetRequestSO(RestfulRequestScriptableObject apiSO)
        {
            req.url     = apiSO.url;
            req.method  = apiSO.method.ToString();
            req.timeout = apiSO.timeout;

            apiSO.SetRequestHeaders(req);

            return this;
        }

        public RestfulBuilder SetUrl(string url)
        {
            req.url = url;
            return this;
        }

        public RestfulBuilder SetMethod(Method method)
        {
            req.method = method.ToString();
            return this;
        }

        public RestfulBuilder SetTimeout(int timeout)
        {
            req.timeout = timeout;
            return this;
        }

        public RestfulBuilder AddHeader(string key, string value)
        {
            req.SetRequestHeader(key, value);
            return this;
        }

        public RestfulBuilder AddQuery(string key, string value)
        {
            req.url += req.url.Contains("?") ? "&" : "?";
            req.url += $"{key}={value}";
            return this;
        }

        public RestfulBuilder SetBody<TRequestData>(TRequestData data)
        {
            var jsonRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            req.uploadHandler = new UploadHandlerRaw(jsonRaw);
            return this;
        }

        /// <summary>
        /// 以 multipart/form-data 送出（檔案上傳）。請在 SetRequestSO/SetMethod 之後呼叫，
        /// 且該請求的 headers 不要放 ContentType —— WWWForm 會自帶含 boundary 的 Content-Type。
        /// </summary>
        public RestfulBuilder SetForm(WWWForm form)
        {
            req.uploadHandler = new UploadHandlerRaw(form.data);

            foreach (var header in form.headers)
                req.SetRequestHeader(header.Key, header.Value);

            return this;
        }

        /// <summary>送出請求；失敗丟例外、成功反序列化為 T。完成後釋放 UnityWebRequest。</summary>
        public async UniTask<TResponseData> StartRequest<TResponseData>()
        {
            using (req)
            {
                await req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                    throw new Exception(req.error);

                return JsonUtility.FromJson<TResponseData>(req.downloadHandler.text);
            }
        }

        /// <summary>送出請求；失敗丟例外、成功回傳原始回應字串（不做 JSON 反序列化）。完成後釋放 UnityWebRequest。</summary>
        public async UniTask<string> StartRequest()
        {
            using (req)
            {
                await req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                    throw new Exception(req.error);

                return req.downloadHandler.text;
            }
        }

        /// <summary>只給 API ScriptableObject 的簡易呼叫。</summary>
        public async UniTask<TResponseData> SimpleRequest<TResponseData>(RestfulRequestScriptableObject apiSO)
        {
            SetRequestSO(apiSO);

            return await StartRequest<TResponseData>();
        }
    }
}
