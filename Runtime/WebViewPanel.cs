using System;
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

        [SerializeField] private MessagePanel messagePanel;

        [SerializeField] private ScreenPadding screenPadding;

        [SerializeField] private UrlConfig urlConfig;
        [Space, SerializeField] public WebViewEvent OnAvatarCreated = new WebViewEvent();
        [SerializeField] public WebViewEvent OnUserSet = new WebViewEvent();
        [SerializeField] public WebViewEvent OnUserAuthorized = new WebViewEvent();

        private WebViewBase webViewObject = null;

        private bool loaded;

        // Event to call when avatar is created, receives GLB url.
        [Serializable] public class WebViewEvent : UnityEvent<string> { }

        /// <summary>
        /// Create WebView object attached to this <see cref="GameObject"/>.
        /// </summary>
        public void LoadWebView()
        {
            var messageType = Application.internetReachability == NetworkReachability.NotReachable ? MessageType.NetworkError : MessageType.Loading;

#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
            messageType = MessageType.NotSupported;
#else
            InitializeAndShowWebView();
#endif
            messagePanel.SetMessage(messageType);
            messagePanel.SetVisible(true);
            SetScreenPadding();
        }

        /// <summary>
        /// Initializes the WebView if it is not already and enables the WebView window.
        /// </summary>
        private async void InitializeAndShowWebView()
        {
            if (webViewObject == null)
            {
#if UNITY_ANDROID
                webViewObject = gameObject.AddComponent<AndroidWebView>();
#elif UNITY_IOS
                webViewObject = gameObject.AddComponent<IOSWebView>();
#endif

                webViewObject.OnLoaded = OnLoaded;
                webViewObject.OnJS = OnWebMessageReceived;

                var options = new WebViewOptions();
                webViewObject.Init(options);
                urlConfig ??= new UrlConfig();
                var url = urlConfig.BuildUrl();
                webViewObject.LoadURL(url);
                webViewObject.IsVisible = true;
            }
            else{
                SetVisible(true);
            }
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
                HandleEvents(JsonConvert.DeserializeObject<WebMessage>(message));
            }
            catch (Exception e)
            {
                SDKLogger.AvatarLoaderLogger.Log(TAG, $"--- Message is not JSON: {message}\nError Message: {e.Message}");
            }
        }
        
        public void SetLastAuthorizedUser(string userId)
        {
            Debug.Log($"USER AUTHORIZED ID = {userId}");
            AccountLinker.SetLastUserId(userId);
        }

        private void HandleEvents(WebMessage webMessage)
        {
            switch (webMessage.eventName)
            {
                case WebViewEventNames.AVATAR_EXPORT:
                    OnAvatarCreated?.Invoke(webMessage.GetAvatarUrl());
                    HideAndClearCache();
                    break;
                case WebViewEventNames.USER_SET:
                    OnUserSet?.Invoke(webMessage.GetUserId());
                    HandleAutoLogin();
                    break;
                case WebViewEventNames.USER_AUTHORIZED:
                    OnUserAuthorized?.Invoke(webMessage.GetUserId());
                    SetLastAuthorizedUser(webMessage.GetUserId());
                    break;
            }
        }

        private async void HandleAutoLogin()
        {
            if (AccountLinker.IsUserSet())
            {
                Debug.Log("PartnerName is Set requesting new token and try auto login");
                var token = await AccountLinker.RequestNewToken("APIKEY");
                var url = urlConfig.BuildUrl(token);
                webViewObject.LoadURL(url);
            }
        }

        private void HideAndClearCache()
        {
            if (urlConfig.clearCache)
            {
                loaded = false;
                webViewObject.Reload();
            }

            SetVisible(false);
        }

        /// <summary>
        /// Receives status message when RPM website is loaded in WebView
        /// </summary>
        /// <param name="message"></param>
        private void OnLoaded(string message)
        {
            if (loaded) return;

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

            loaded = true;
        }
    }
}
