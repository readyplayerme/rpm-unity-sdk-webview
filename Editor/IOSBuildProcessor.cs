#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace ReadyPlayerMe.WebView.Editor
{
    public class UnityWebViewPostprocessBuild
    {

        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
        {
            if (buildTarget != BuildTarget.iOS) return;

            var projectPath = $"{path}/Unity-iPhone.xcodeproj/project.pbxproj";

            var pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
            
            // Main
            var targetGuid = pbxProject.GetUnityMainTargetGuid();
            pbxProject.AddFrameworkToProject(targetGuid, "WebKit.framework", false);

            pbxProject.WriteToFile(projectPath);
        }
    }
}
#endif
