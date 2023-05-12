using System;
using UnityEngine.Events;

namespace ReadyPlayerMe.WebView
{
    // Event to call when avatar is created, receives GLB url.
    [Serializable] public class WebViewEvent : UnityEvent<string>
    {
    }

    // Event to call when avatar is created, receives GLB url.
    [Serializable] public class AssetUnlockEvent : UnityEvent<AssetRecord>
    {
    }

    public static class WebViewEvents
    {
        public const string AVATAR_EXPORT = "v1.avatar.exported";
        public const string USER_SET = "v1.user.set";
        public const string USER_AUTHORIZED = "v1.user.authorized";
        public const string ASSET_UNLOCK = "v1.asset.unlock";
    }
}
