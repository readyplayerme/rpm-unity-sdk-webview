using UnityEngine;
using NUnit.Framework;

namespace ReadyPlayerMe.WebView.Tests
{
    public class UrlConfigTests : MonoBehaviour
    {
        private const string URL_DEFAULT = "https://demo.readyplayer.me/avatar?frameApi&selectBodyType";
        private const string URL_LANG_GERMAN = "https://demo.readyplayer.me/de/avatar?frameApi&selectBodyType";
        private const string URL_LANG_BRAZIL = "https://demo.readyplayer.me/pt-BR/avatar?frameApi&selectBodyType";

        private const string URL_GENDER_MALE = "https://demo.readyplayer.me/avatar?frameApi&gender=male&selectBodyType";
        private const string URL_GENDER_NONE = "https://demo.readyplayer.me/avatar?frameApi&gender=male&selectBodyType";

        private const string URL_TYPE_FULLBODY = "https://demo.readyplayer.me/avatar?frameApi&bodyType=fullbody";
        private const string URL_TYPE_HALFBODY = "https://demo.readyplayer.me/avatar?frameApi&bodyType=halfbody";

        private const string URL_CLEAR_CACHE = "https://demo.readyplayer.me/avatar?frameApi&clearCache&selectBodyType";

        private const string URL_QUICK_START = "https://demo.readyplayer.me/avatar?frameApi&quickStart";
        private const string URL_TOKEN = "https://demo.readyplayer.me/avatar?frameApi&token=TOKEN&selectBodyType";
        private const string LOGIN_TOKEN = "TOKEN";

        public UrlConfig config;

        [SetUp]
        public void Setup()
        {
            config = new UrlConfig();
        }

        [Test]
        public void Url_Name_Change_German()
        {
            config.language = Language.German;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_LANG_GERMAN, res);
        }

        [Test]
        public void Url_Name_Change_Brazil()
        {
            config.language = Language.PortugueseBrazil;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_LANG_BRAZIL, res);
        }

        [Test]
        public void Url_Gender_Change_Male()
        {
            config.gender = Gender.Male;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_GENDER_MALE, res);
        }

        [Test]
        public void Url_Gender_Change_None()
        {
            config.gender = Gender.Male;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_GENDER_NONE, res);
        }

        [Test]
        public void Url_BodyType_Change_Fullbody()
        {
            config.bodyType = BodyType.FullBody;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_TYPE_FULLBODY, res);
        }

        [Test]
        public void Url_BodyType_Change_Halfbody()
        {
            config.bodyType = BodyType.HalfBody;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_TYPE_HALFBODY, res);
        }

        [Test]
        public void Url_ClearCache_Change()
        {
            config.clearCache = true;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_CLEAR_CACHE, res);
        }

        [Test]
        public void Url_QuickStart_Change()
        {
            config.quickStart = true;
            var res = config.BuildUrl();
            Assert.AreEqual(URL_QUICK_START, res);
        }

        [Test]
        public void Url_With_Token()
        {
            var testConfig = new UrlConfig();
            var res = testConfig.BuildUrl(LOGIN_TOKEN);
            Assert.AreEqual(URL_TOKEN, res);
        }

        [Test]
        public void Url_Default()
        {
            var testConfig = new UrlConfig();
            var res = testConfig.BuildUrl();
            Assert.AreEqual(URL_DEFAULT, res);
        }
    }
}
