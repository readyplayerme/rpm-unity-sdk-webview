using System;
using System.Text;
using Newtonsoft.Json;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.WebView
{
    /// <summary>
    /// This class is used to define all the settings related to the URL that is used for creating the URL to be loaded in the WebView browser.
    /// </summary>
    [System.Serializable]
    public class UrlConfig
    {
        private const string TAG = nameof(UrlConfig);
        private const string CLEAR_CACHE_PARAM = "clearCache";
        private const string FRAME_API_PARAM = "frameApi";
        private const string QUICK_START_PARAM = "quickStart";
        private const string SELECT_BODY_PARAM = "selectBodyType";
        private const string LOGIN_TOKEN_PARAM = "token";
      
        [Tooltip("Language of the RPM website.")]
        public Language language = Language.Default;

        [Tooltip("Check if you want user to create a new avatar every visit. If not checked, avatar editor will continue from previously created avatar.")]
        public bool clearCache;

        [Tooltip("Start with preset full-body avatars. Checking this option makes avatar editor ignore Gender and Body Type options.")]
        public bool quickStart;

        [Tooltip("Skip gender selection and create avatars with selected gender. Ignored if Quick Start is checked.")]
        public Gender gender = Gender.None;

        [Tooltip("Skip body type selection and create avatars with selected body type. Ignored if Quick Start is checked.")]
        public BodyType bodyType = BodyType.Selectable;
      
        /// <summary>
        /// Builds RPM website URL for partner with given parameters.
        /// </summary>
        /// <returns>The Url to load in the WebView.</returns>
        public string BuildUrl(string loginToken = "")
        {
            var builder = new StringBuilder($"https://{CoreSettingsHandler.CoreSettings.Subdomain}.readyplayer.me/");
            builder.Append(language != Language.Default ? $"{language.GetValue()}/" : string.Empty);
            builder.Append($"avatar?{FRAME_API_PARAM}");
            builder.Append(clearCache ? $"&{CLEAR_CACHE_PARAM}" : string.Empty);
            if (loginToken != string.Empty)
            {
                builder.Append($"&{LOGIN_TOKEN_PARAM}={loginToken}");
            }

            if (quickStart)
            {
                builder.Append($"&{QUICK_START_PARAM}");
            }
            else
            {
                builder.Append(gender != Gender.None ? $"&gender={gender.GetValue()}" : string.Empty);
                builder.Append(bodyType == BodyType.Selectable ? $"&{SELECT_BODY_PARAM}" : $"&bodyType={bodyType.GetValue()}");
            }

            var url = builder.ToString();
            SDKLogger.AvatarLoaderLogger.Log(TAG, url);

            return url;
        }
    }
}
