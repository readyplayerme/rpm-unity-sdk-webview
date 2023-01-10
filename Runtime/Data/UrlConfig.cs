using UnityEngine;

namespace ReadyPlayerMe.WebView
{
    /// <summary>
    /// This class is used to define all the settings related to the URL that is used for creating the URL to be loaded in the WebView browser.
    /// </summary>
    [System.Serializable]
    public class UrlConfig
    {
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
    }
}
