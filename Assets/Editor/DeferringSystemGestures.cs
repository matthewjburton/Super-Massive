using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class DeferringSystemGestures
{
#if UNITY_CLOUD_BUILD
    // This method is added in the Advanced Features Settings on UCB
    // PostBuildProcessor.OnPostprocessBuildiOS
    public static void OnPostprocessBuildiOS (string exportPath)
    {
        Debug.Log("[UCB] OnPostprocessBuildiOS");
        ProcessPostBuild(BuildTarget.iPhone,exportPath);
    }
#endif

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
#if !UNITY_CLOUD_BUILD
        Debug.Log("[iOS] OnPostprocessBuild");
        ProcessPostBuild(buildTarget, path);
#endif
    }

    public static void ProcessPostBuild(BuildTarget buildTarget, string path)
    {
#if UNITY_IOS
        if (buildTarget == BuildTarget.iOS)
        {
            var placement = "- (BOOL)prefersStatusBarHidden";
#if UNITY_2017_2_OR_NEWER
            var fileBase = "/Classes/UI/UnityViewControllerBase+iOS";
#else
            var fileBase = "/Classes/UI/UnityViewControllerBaseiOS";
#endif

            AddToFile(
                path: path + fileBase + ".h",
                location: placement,
                text: "- (UIRectEdge)preferredScreenEdgesDeferringSystemGestures;");

            AddToFile(
                path: path + fileBase + ".mm",
                location: placement,
                text: "- (UIRectEdge)preferredScreenEdgesDeferringSystemGestures\n{\n return UIRectEdgeAll;\n}\n");
        }
#endif
    }

    static void AddToFile(string path, string text, string location)
    {
        string content = System.IO.File.ReadAllText(path);

        if (!content.Contains(text))
        {
            content = content.Replace(location, text + "\n" + location);
        }
        System.IO.File.WriteAllText(path, content);
    }
}