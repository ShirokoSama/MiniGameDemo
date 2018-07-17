using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetsBundle : MonoBehaviour {

	[MenuItem("Assets/Build AssetBundles")]
    public static void BuildAssetBundle()
    {
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 38; j++)
            {
                AssetImporter importer = AssetImporter.GetAtPath("Assets/Sprites/Background/BackgroundSplit-" + i + "-" + j + ".png");
                importer.assetBundleName = "Background/BackgroundSplit-" + i + "-" + j;
                importer.assetBundleVariant = "normal";

            }
        }

        bool isExist = Directory.Exists(Application.dataPath + "/StreamingAssets");
        if (!isExist)
        {
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        }

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }


    [MenuItem("Assets/Temp Clear")]
    public static void Clear()
    {
        AssetDatabase.RemoveAssetBundleName("backgroundsplit-16-37.png.normal", true);
    }
}
