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
            
            //Main
            var targetGuid = pbxProject.GetUnityMainTargetGuid();
            pbxProject.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
            pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            
            //Unity Tests
            targetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
            pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            
            //Unity Framework
            targetGuid = pbxProject.GetUnityFrameworkTargetGuid();
            pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ld_classic");

            pbxProject.WriteToFile(projectPath);
        }
    }
}
#endif
