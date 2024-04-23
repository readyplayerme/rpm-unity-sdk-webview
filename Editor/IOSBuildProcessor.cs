#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace ReadyPlayerMe.WebView.Editor
{
    public class IOSBuildProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS) return;

            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var type = Type.GetType("UnityEditor.iOS.Xcode.PBXProject, UnityEditor.iOS.Extensions.Xcode");
            if (type == null)
            {
                Debug.LogError("unitywebview: failed to get PBXProject. please install iOS build support.");
                return;
            }
            var src = File.ReadAllText(projPath);
            //dynamic proj = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            var proj = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            //proj.ReadFromString(src);
            {
                var method = type.GetMethod("ReadFromString");
                method.Invoke(proj, new object[]{src});
            }
            var target = "";
#if UNITY_2019_3_OR_NEWER
            //target = proj.GetUnityFrameworkTargetGuid();
            {
                var method = type.GetMethod("GetUnityFrameworkTargetGuid");
                target = (string)method.Invoke(proj, null);
            }
#else
            //target = proj.TargetGuidByName("Unity-iPhone");
            {
                var method = type.GetMethod("TargetGuidByName");
                target = (string)method.Invoke(proj, new object[]{"Unity-iPhone"});
            }
#endif
            //proj.AddFrameworkToProject(target, "WebKit.framework", false);
            {
                var method = type.GetMethod("AddFrameworkToProject");
                method.Invoke(proj, new object[]{target, "WebKit.framework", false});
            }
            var cflags = "";
            if (EditorUserBuildSettings.development) {
                cflags += " -DUNITYWEBVIEW_DEVELOPMENT";
            }
#if UNITYWEBVIEW_IOS_ALLOW_FILE_URLS
            cflags += " -DUNITYWEBVIEW_IOS_ALLOW_FILE_URLS";
#endif
            cflags = cflags.Trim();
            if (!string.IsNullOrEmpty(cflags)) {
                // proj.AddBuildProperty(target, "OTHER_LDFLAGS", cflags);
                var method = type.GetMethod("AddBuildProperty", new Type[]{typeof(string), typeof(string), typeof(string)});
                method.Invoke(proj, new object[]{target, "OTHER_CFLAGS", cflags});
            }
            var dst = "";
            //dst = proj.WriteToString();
            {
                var method = type.GetMethod("WriteToString");
                dst = (string)method.Invoke(proj, null);
            }
            File.WriteAllText(projPath, dst);
        }
    }
}
#endif
