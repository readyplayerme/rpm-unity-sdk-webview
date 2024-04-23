#if UNITY_IOS
using System;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.WebView.Editor
{
    public class IOSBuildProcessor
    {

        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.iOS) return;

            var projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var type = Type.GetType("UnityEditor.iOS.Xcode.PBXProject, UnityEditor.iOS.Extensions.Xcode");
            if (type == null)
            {
                Debug.LogError("unitywebview: failed to get PBXProject. please install iOS build support.");
                return;
            }
            var src = File.ReadAllText(projPath);
            var proj = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            {
                var method = type.GetMethod("ReadFromString");
                method.Invoke(proj, new object[] { src });
            }
            var target = "";
            {
                var method = type.GetMethod("GetUnityFrameworkTargetGuid");
                target = (string) method.Invoke(proj, null);
            }

            {
                var method = type.GetMethod("AddFrameworkToProject");
                method.Invoke(proj, new object[] { target, "WebKit.framework", false });
            }
            var cflags = "";
            if (EditorUserBuildSettings.development)
            {
                cflags += " -DUNITYWEBVIEW_DEVELOPMENT";
            }
#if UNITYWEBVIEW_IOS_ALLOW_FILE_URLS
            cflags += " -DUNITYWEBVIEW_IOS_ALLOW_FILE_URLS";
#endif
            cflags = cflags.Trim();
            if (!string.IsNullOrEmpty(cflags))
            {
                var method = type.GetMethod("AddBuildProperty", new Type[] { typeof(string), typeof(string), typeof(string) });
                method.Invoke(proj, new object[] { target, "OTHER_CFLAGS", cflags });
            }
            var dst = "";
            {
                var method = type.GetMethod("WriteToString");
                dst = (string) method.Invoke(proj, null);
            }
            File.WriteAllText(projPath, dst);
        }
    }
}
#endif
