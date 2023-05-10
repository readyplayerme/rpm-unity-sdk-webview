#if UNITY_EDITOR
using System.IO;
using System.Xml;
using UnityEditor;
using System.Text;
using UnityEditor.Android;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace ReadyPlayerMe.WebView.Editor
{
    /// <summary>
    /// Receives a callback after the Android Gradle project is generated,
    /// and the callback is used for generating a manifest file with required permissions.
    /// </summary>
    public class WebViewBuildPostprocessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 1;

        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            var manifestPath = GetManifestPath(basePath);
            var androidManifest = new AndroidManifest(manifestPath);

            androidManifest
                .SetHardwareAccelerated(true)
                .SetUsesCleartextTraffic(true)
                .UseCamera()
                .UseMicrophone()
                .UseGallery()
                .AllowBackup();

            androidManifest.Save();
        }

        private string GetManifestPath(string basePath)
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            return pathBuilder.ToString();
        }

        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS) {
                string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
                PBXProject proj = new PBXProject();
                proj.ReadFromString(File.ReadAllText(projPath));
                proj.AddFrameworkToProject(proj.GetUnityFrameworkTargetGuid(), "WebKit.framework", false);
                File.WriteAllText(projPath, proj.WriteToString());
            }
#endif
        }
    }

    /// <summary>
    /// AndroidManifest.xml file that is created with necessary permissions.
    /// </summary>
    internal class AndroidXmlDocument : XmlDocument
    {
        private string manifestPath;
        protected XmlNamespaceManager namespaceManager;
        public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
        public readonly string ToolsXmlNamespace = "http://schemas.android.com/tools";

        public AndroidXmlDocument(string path)
        {
            manifestPath = path;
            using (var reader = new XmlTextReader(manifestPath))
            {
                reader.Read();
                Load(reader);
            }

            namespaceManager = new XmlNamespaceManager(NameTable);
            namespaceManager.AddNamespace("android", AndroidXmlNamespace);
            namespaceManager.AddNamespace("tools", ToolsXmlNamespace);
        }

        public string Save()
        {
            return SaveAs(manifestPath);
        }

        public string SaveAs(string path)
        {
            using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                Save(writer);
            }

            return path;
        }
    }

    internal class AndroidManifest : AndroidXmlDocument
    {
        private const string NodeKey = "name";
        private const string UsesFeature = "uses-feature";
        private const string UsesPermission = "uses-permission";

        private const string CameraPermission = "android.permission.CAMERA";
        private const string CameraFeature = "android.hardware.camera";

        private const string MicrophonePermission = "android.permission.MICROPHONE";
        private const string MicrophoneFeature = "android.hardware.microphone";

        private const string ReadExternalStoragePermission = "android.permission.READ_EXTERNAL_STORAGE";
        private const string WriteExternalStoragePermission = "android.permission.Write_EXTERNAL_STORAGE";

        private const string UsesCleartextTrafficAttribute = "usesCleartextTraffic";
        private const string HardwareAcceleratedAttribute = "hardwareAccelerated";

        private const string XPath = "/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and intent-filter/category/@android:name='android.intent.category.LAUNCHER']";

        private static XmlNode ActivityWithLaunchIntent = null;

        private readonly XmlElement ManifestElement;

        public AndroidManifest(string path) : base(path)
        {
            ManifestElement = SelectSingleNode("/manifest") as XmlElement;
        }

        internal XmlNode GetActivityWithLaunchIntent()
        {
            return ActivityWithLaunchIntent ?? SelectSingleNode(XPath, namespaceManager);
        }

        #region Node Edit Methods

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }

        private XmlAttribute CreateToolsAttribute(string key, string value)
        {
            XmlAttribute attr = CreateAttribute("tools", key, ToolsXmlNamespace);
            attr.Value = value;
            return attr;
        }

        internal void UpdateAttribute(XmlElement activity, string attribute, bool enabled)
        {
            var value = enabled.ToString();

            if (activity.GetAttribute(attribute, AndroidXmlNamespace) != value)
            {
                activity.SetAttribute(attribute, AndroidXmlNamespace, value);
            }
        }

        internal void UpdateNode(string nodeName, string nodeValue)
        {
            XmlNodeList node = SelectNodes($"/manifest/{nodeName}[@android:{NodeKey}='{nodeValue}']", namespaceManager);
            if (node?.Count == 0)
            {
                XmlElement elem = CreateElement(nodeName);
                elem.Attributes.Append(CreateAndroidAttribute(NodeKey, nodeValue));
                ManifestElement.AppendChild(elem);
            }
        }

        internal void UseFeature(string feature)
        {
            UpdateNode(UsesFeature, feature);
        }

        internal void UsePermission(string permission)
        {
            UpdateNode(UsesPermission, permission);
        }

        #endregion

        #region AndroidManifest Options

        internal AndroidManifest SetUsesCleartextTraffic(bool enabled)
        {
            var activity = GetActivityWithLaunchIntent() as XmlElement;
            UpdateAttribute(activity, UsesCleartextTrafficAttribute, enabled);
            return this;
        }

        internal AndroidManifest AllowBackup()
        {
            XmlNode elem = SelectSingleNode("/manifest/application");
            if (elem?.Attributes != null)
            {
                elem.Attributes.Append(CreateAndroidAttribute("allowBackup", "false"));
                elem.Attributes.Append(CreateToolsAttribute("replace", "android:allowBackup"));
            }

            return this;
        }

        internal AndroidManifest SetHardwareAccelerated(bool enabled)
        {
            var activity = GetActivityWithLaunchIntent() as XmlElement;
            UpdateAttribute(activity, HardwareAcceleratedAttribute, enabled);
            return this;
        }

        internal AndroidManifest UseCamera()
        {
            UsePermission(CameraPermission);
            UseFeature(CameraFeature);
            return this;
        }

        internal AndroidManifest UseMicrophone()
        {
            UsePermission(MicrophonePermission);
            UseFeature(MicrophoneFeature);
            return this;
        }

        internal AndroidManifest UseGallery()
        {
            UsePermission(ReadExternalStoragePermission);
            UsePermission(WriteExternalStoragePermission);
            return this;
        }

        #endregion
    }
}
#endif
