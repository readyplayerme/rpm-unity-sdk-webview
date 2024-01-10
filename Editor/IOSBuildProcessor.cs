#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace ReadyPlayerMe.WebView.Editor
{
    public class IOSBuildProcessor
    {
        [PostProcessBuildAttribute(100)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (BuildTarget.iOS != target) return;
            var projectPath = $"{pathToBuiltProject}/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projectPath));
            proj.AddFrameworkToProject(proj.TargetGuidByName("Unity-iPhone"), "WebKit.framework", false);
            File.WriteAllText(projectPath, proj.WriteToString());
        }
    }
}
#endif
