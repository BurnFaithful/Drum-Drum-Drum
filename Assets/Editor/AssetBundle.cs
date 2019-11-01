using System.IO;
using UnityEngine;
using UnityEditor;

public class AssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAssetBundles()
    {
        string directoryPath = Path.Combine(Application.dataPath, "StreamingAssets", "AssetBundles");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
