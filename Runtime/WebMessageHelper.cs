namespace ReadyPlayerMe.WebView
{
    public struct AssetRecord
    {
        public string UserId;
        public string AssetId;
    }

    public static class WebMessageHelper
    {
        private const string DATA_URL_FIELD_NAME = "url";
        private const string ID_KEY = "id";
        private const string USER_ID_KEY = "userId";
        private const string ASSET_ID_KEY = "assetId";

        public static string GetAvatarUrl(this WebMessage webMessage)
        {
            webMessage.data.TryGetValue(DATA_URL_FIELD_NAME, out var avatarUrl);
            return avatarUrl ?? string.Empty;
        }

        public static string GetUserId(this WebMessage webMessage)
        {
            webMessage.data.TryGetValue(ID_KEY, out var userId);
            return userId ?? string.Empty;
        }

        public static AssetRecord GetAssetRecord(this WebMessage webMessage)
        {
            webMessage.data.TryGetValue(ASSET_ID_KEY, out var assetId);
            webMessage.data.TryGetValue(USER_ID_KEY, out var userId);
            var assetRecord = new AssetRecord();
            assetRecord.AssetId = assetId;
            assetRecord.UserId = userId;
            return assetRecord;
        }
    }
}
