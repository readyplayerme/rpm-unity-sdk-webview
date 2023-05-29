using UnityEngine;
using NUnit.Framework;

namespace ReadyPlayerMe.WebView.Tests
{
    public class UrlConfigTests : MonoBehaviour
    {
        private const string LANG_GERMAN = "https://demo.readyplayer.me/de/avatar?frameApi&selectBodyType";
        private const string LANG_BRAZIL = "https://demo.readyplayer.me/pt-BR/avatar?frameApi&selectBodyType";

        private const string GENDER_MALE = "https://demo.readyplayer.me/avatar?frameApi&gender=male&selectBodyType";
        private const string GENDER_NONE = "https://demo.readyplayer.me/avatar?frameApi&gender=male&selectBodyType";

        private const string TYPE_FULLBODY = "https://demo.readyplayer.me/avatar?frameApi&bodyType=fullbody";
        private const string TYPE_HALFBODY = "https://demo.readyplayer.me/avatar?frameApi&bodyType=halfbody";

        private const string CLEAR_CACHE = "https://demo.readyplayer.me/avatar?frameApi&clearCache&selectBodyType";

        private const string QUICK_START = "https://demo.readyplayer.me/avatar?frameApi&quickStart";

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
            Assert.AreEqual(LANG_GERMAN, res);
        }

        [Test]
        public void Url_Name_Change_Brazil()
        {
            config.language = Language.PortugueseBrazil;
            var res = config.BuildUrl();
            Assert.AreEqual(LANG_BRAZIL, res);
        }

        [Test]
        public void Url_Gender_Change_Male()
        {
            config.gender = Gender.Male;
            var res = config.BuildUrl();
            Assert.AreEqual(GENDER_MALE, res);
        }

        [Test]
        public void Url_Gender_Change_None()
        {
            config.gender = Gender.Male;
            var res = config.BuildUrl();
            Assert.AreEqual(GENDER_NONE, res);
        }

        [Test]
        public void Url_BodyType_Change_Fullbody()
        {
            config.bodyType = BodyType.FullBody;
            var res = config.BuildUrl();
            Assert.AreEqual(TYPE_FULLBODY, res);
        }

        [Test]
        public void Url_BodyType_Change_Halfbody()
        {
            config.bodyType = BodyType.HalfBody;
            var res = config.BuildUrl();
            Assert.AreEqual(TYPE_HALFBODY, res);
        }

        [Test]
        public void Url_ClearCache_Change()
        {
            config.clearCache = true;
            var res = config.BuildUrl();
            Assert.AreEqual(CLEAR_CACHE, res);
        }

        [Test]
        public void Url_QuickStart_Change()
        {
            config.quickStart = true;
            var res = config.BuildUrl();
            Assert.AreEqual(QUICK_START, res);
        }
    }
}
