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

            var projectPath = $"{report.summary.outputPath}/Unity-iPhone.xcodeproj/project.pbxproj";

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
