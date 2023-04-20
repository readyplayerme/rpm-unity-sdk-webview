using System;
using System.Text;
using Newtonsoft.Json;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.WebView
{
    /// <summary>
    /// This class is responsible for displaying and updating the WebView UI panel and <see cref="MessagePanel" />.
    /// </summary>
    public class WebViewPanel : MonoBehaviour
    {
        private const string TAG = nameof(WebViewPanel);
        private const string DATA_URL_FIELD_NAME = "url";
        private const string AVATAR_EXPORT_EVENT_NAME = "v1.avatar.exported";
        private const string CLEAR_CACHE_PARAM = "clearCache";
        private const string FRAME_API_PARAM = "frameApi";
        private const string QUICK_START_PARAM = "quickStart";
        private const string SELECT_BODY_PARAM = "selectBodyType";

        [SerializeField] private MessagePanel messagePanel;

        [SerializeField] private ScreenPadding screenPadding;

        [SerializeField] private UrlConfig urlConfig;
        [Space, SerializeField] public AvatarCreatedEvent OnAvatarCreated = new AvatarCreatedEvent();

        private WebViewBase webViewObject = null;
        
        public bool Loaded { get; private set; }

        // Event to call when avatar is created, receives GLB url.
        [Serializable] public class AvatarCreatedEvent : UnityEvent<string> { }

        /// <summary>
        /// Create WebView object attached to this <see cref="GameObject"/>.
        /// </summary>
        public void LoadWebView()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                messagePanel.SetMessage(MessageType.NetworkError);
                messagePanel.SetVisible(true);
            }
            else
            {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
                messagePanel.SetMessage(MessageType.NotSupported);
                messagePanel.SetVisible(true);
#else
                    if (webViewObject == null)
                    {
                        messagePanel.SetMessage(MessageType.Loading);
                        messagePanel.SetVisible(true);

                        #if UNITY_ANDROID
                            webViewObject = gameObject.AddComponent<AndroidWebView>();
                        #elif UNITY_IOS
                            webViewObject = gameObject.AddComponent<IOSWebView>();
                        #endif

                        webViewObject.OnLoaded = OnLoaded;
                        webViewObject.OnJS = OnWebMessageReceived;

                        WebViewOptions options = new WebViewOptions();
                        webViewObject.Init(options);

                        string url = GetUrlFromConfig();
                        webViewObject.LoadURL(url);
                        webViewObject.IsVisible = true;
                    }
                    else{
                        SetVisible(true);
                    }
#endif
            }

            SetScreenPadding();
        }

        /// <summary>
        /// Set the WebView Panel visibility.
        /// </summary>
        public void SetVisible(bool visible)
        {
            messagePanel.SetVisible(visible);
            if (webViewObject != null)
            {
                webViewObject.IsVisible = visible;
            }
        }

        private void OnDrawGizmos()
        {
            var rectTransform = transform as RectTransform;
            if (rectTransform != null)
            {
                Gizmos.matrix = rectTransform.localToWorldMatrix;
                Gizmos.color = Color.green;

                var center = new Vector3((screenPadding.left - screenPadding.right) / 2f, (screenPadding.bottom - screenPadding.top) / 2f);
                Rect rect = rectTransform.rect;
                var size = new Vector3(rect.width - (screenPadding.left + screenPadding.right), rect.height - (screenPadding.bottom + screenPadding.top));

                Gizmos.DrawWireCube(center, size);
            }
        }


        /// <summary>
        /// Builds RPM website URL for partner with given parameters.
        /// </summary>
        /// <returns>The Url to load in the WebView.</returns>
        private string GetUrlFromConfig()
        {
            var partnerSubdomain = CoreSettingsHandler.CoreSettings.Subdomain;
            var builder = new StringBuilder($"https://{partnerSubdomain}.readyplayer.me/");
            builder.Append(urlConfig.language != Language.Default ? $"{urlConfig.language.GetValue()}/" : string.Empty);
            builder.Append($"avatar?{FRAME_API_PARAM}");
            builder.Append(urlConfig.clearCache ? $"&{CLEAR_CACHE_PARAM}" : string.Empty);

            if (urlConfig.quickStart)
            {
                builder.Append(QUICK_START_PARAM);
            }
            else
            {
                builder.Append(urlConfig.gender != Gender.None ? $"&gender={urlConfig.gender.GetValue()}" : string.Empty);
                builder.Append(urlConfig.bodyType == BodyType.Selectable ? $"&{SELECT_BODY_PARAM}" : $"&bodyType={urlConfig.bodyType.GetValue()}");
            }

            var url = builder.ToString();
            SDKLogger.AvatarLoaderLogger.Log(TAG, "url");

            return url;
        }

        // Set WebView screen padding in pixels.
        private void SetScreenPadding()
        {
            if (webViewObject)
            {
                webViewObject.SetMargins(screenPadding.left, screenPadding.top, screenPadding.right, screenPadding.bottom);
            }

            messagePanel.SetMargins(screenPadding.left, screenPadding.top, screenPadding.right, screenPadding.bottom);
        }

        // Receives message from RPM website, which contains avatar URL.
        private void OnWebMessageReceived(string message)
        {
            SDKLogger.AvatarLoaderLogger.Log(TAG, $"--- WebView Message: {message}");
            try
            {
                var webMessage = JsonConvert.DeserializeObject<WebMessage>(message);

                if (webMessage.eventName == AVATAR_EXPORT_EVENT_NAME)
                {
                    if (webMessage.data.TryGetValue(DATA_URL_FIELD_NAME, out var avatarUrl))
                    {
                        OnAvatarCreated?.Invoke(avatarUrl);

                        if (urlConfig.clearCache)
                        {
                            Loaded = false;
                            webViewObject.Reload();
                        }

                        SetVisible(false);
                    }
                }
            }
            catch (Exception e)
            {
                SDKLogger.AvatarLoaderLogger.Log(TAG, $"--- Message is not JSON: {message}\nError Message: {e.Message}");
            }
        }

        /// <summary>
        /// Receives status message when RPM website is loaded in WebView
        /// </summary>
        /// <param name="message"></param>
        private void OnLoaded(string message)
        {
            if (Loaded) return;

            SDKLogger.AvatarLoaderLogger.Log(TAG, $"--- WebView Loaded.");
            webViewObject.EvaluateJS(@"
                document.cookie = 'webview=true';

                if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                        call: function(msg) { 
                            window.webkit.messageHandlers.unityControl.postMessage(msg); 
                        }
                    }
                }
                else {
                    window.Unity = {
                        call: function(msg) {
                            window.location = 'unity:' + msg;
                        }
                    }
                }

                function subscribe(event) {
                    const json = parse(event);
                    const source = json.source;
                        
                    if (source !== 'readyplayerme') {
                        return;
                    }

			        Unity.call(event.data);
		        }

                function parse(event) {
                    try {
                        return JSON.parse(event.data);
                    } catch (error) {
                        return null;
                    }
                }

                window.postMessage(
                    JSON.stringify({
                        target: 'readyplayerme',
                        type: 'subscribe',
                        eventName: 'v1.**'
                    }),
                    '*'
                );

                window.removeEventListener('message', subscribe);
                window.addEventListener('message', subscribe)
            ");

            Loaded = true;
        }
    }
}
