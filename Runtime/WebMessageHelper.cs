namespace ReadyPlayerMe.WebView
{
    public static class WebMessageHelper
    {
        private const string DATA_URL_FIELD_NAME = "url";
        private const string USER_ID_KEY = "id";

        public static string GetAvatarUrl(this WebMessage webMessage)
        {
            webMessage.data.TryGetValue(DATA_URL_FIELD_NAME, out var avatarUrl);
            return avatarUrl ?? string.Empty;
        }

        public static string GetUserId(this WebMessage webMessage)
        {
            webMessage.data.TryGetValue(USER_ID_KEY, out var userId);
            return userId ?? string.Empty;
        }
    }
}
