#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.WebView.Editor
{
    public class WebViewEditor : UnityEditor.Editor
    {
        private const string WEB_VIEW_CANVAS_FILE_NAME = "WebView Canvas";

        /// <summary>
        /// Loads a WebView Canvas prefab to the current scene.
        /// </summary>
        [MenuItem("GameObject/UI/Ready Player Me/WebView Canvas", false)]
        private static void LoadWebViewCanvas()
        {
            var prefab = Resources.Load<GameObject>(WEB_VIEW_CANVAS_FILE_NAME);
            GameObject instance = Instantiate(prefab);
            instance.name = WEB_VIEW_CANVAS_FILE_NAME;
            Selection.activeGameObject = instance;
            EditorUtility.SetDirty(instance);
        }
    }
}
#endif
