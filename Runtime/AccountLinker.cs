using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReadyPlayerMe.Core;
using UnityEngine;
using Response = ReadyPlayerMe.Core.Response;
using WebRequestDispatcher = ReadyPlayerMe.Core.WebRequestDispatcher;

namespace ReadyPlayerMe.WebView
{
    // TODO refactor to use UserSession from avatar Creator
    [Serializable]
    public struct UserSession
    {
        [JsonProperty("_id")]
        public string Id;
        public string Token;
    }
    
    public static class AccountLinker
    {
        private const string LAST_USER_ID_PREF = "RPM_LAST_USER_ID";
        private const string URL_PREFIX = "https://readyplayer.me/api/auth/token";

        public static async Task<string> RequestNewToken(string apiKey)
        {
            return await RequestNewToken(GetLastUserId(), apiKey);
        }
        
        public static async Task<string> RequestNewToken(string userId, string apiKey)
        {
            var url = $"{URL_PREFIX}?userId={userId}&partner={CoreSettingsHandler.CoreSettings.Subdomain}";
            Debug.Log($"Requesting new token from {url}");
            var dispatcher = new WebRequestDispatcher();
            var headers = new Dictionary<string, string>();
            headers.Add("x-api-key", apiKey);
            var response = await dispatcher.SendRequest<Response>(url, HttpMethod.GET, headers);
            if (!response.IsSuccess)
            {
                throw new Exception();
            }
            var userSession = JsonConvert.DeserializeObject<UserSession>(response.Data.ToString());
            return userSession.Token;
        }

        public static string GetLastUserId()
        {
            return PlayerPrefs.GetString(LAST_USER_ID_PREF, string.Empty);
        }

        public static void SetLastUserId(string userId)
        {
            PlayerPrefs.SetString(LAST_USER_ID_PREF, userId);
        }

        public static void ClearLastUserId()
        {
            SetLastUserId(string.Empty);
        }

        public static bool IsUserSet()
        {
            return GetLastUserId() != String.Empty;
        }
    }
}
