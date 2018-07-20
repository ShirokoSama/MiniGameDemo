using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetsBundle : MonoBehaviour {

	[MenuItem("Assets/Build AssetBundles")]
    public static void BuildAssetBundle()
    {
        bool isExist = Directory.Exists(Application.dataPath + "/StreamingAssets");
        if (!isExist)
        {
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        }

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }

    [MenuItem("Assets/Set Background AssetBundle")]
    public static void SetBackgroundAssetBundle()
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
    }

    [MenuItem("Assets/Set Map Object AssetBundle")]
    public static void SetMapObjectAssetBundle()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Prefabs/MapComponents", "*.prefab");
        string relativePath = Application.dataPath + "/Prefabs/MapComponents";
        int relativeLen = relativePath.Length;
        foreach (string file in files)
        {
            string fileName = file.Substring(relativeLen + 1);
            AssetImporter importer = AssetImporter.GetAtPath("Assets/Prefabs/MapComponents/" + fileName);
            importer.assetBundleName = "mapComponents/" + fileName.Substring(0, fileName.Length - 7);
            importer.assetBundleVariant = "normal";
        }
    }


    [MenuItem("Assets/Temp Clear")]
    public static void Clear()
    {
        AssetDatabase.RemoveAssetBundleName("backgroundsplit-16-37.png.normal", true);
    }
}
