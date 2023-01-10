namespace ReadyPlayerMe.WebView
{
    public class WebViewOptions
    {
        public bool Transparent = true;
        public bool Zoom = false;
        public bool EnableWKWebView = true;
        public string UserAgent = string.Empty;
        public ColorMode ColorMode = ColorMode.DarkModeOff;
        public WebkitContentMode ContentMode = WebkitContentMode.Recommended;
    }
}
