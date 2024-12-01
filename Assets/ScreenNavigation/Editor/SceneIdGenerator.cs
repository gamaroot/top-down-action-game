#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SceneIdGenerator : MonoBehaviour
{
    private const string PATH = "Assets/ScreenNavigation/SceneID.cs";

    // Menu item to manually regenerate the enum file
    [MenuItem("Tools/Update Scene ID Enum")]
    public static void GenerateSceneIdEnum()
    {
        // Build the enum content
        string enumContent = "using System;\n\n" +
                             "namespace ScreenNavigation\n" +
                             "{\n" +
                                 "\t[Serializable]\n" +
                                 "\tpublic enum SceneID \n" +
                                 "\t{\n";

        var scenes = EditorBuildSettings.scenes;
        for (int index = 0; index < scenes.Length; index++)
        {
            string scenePath = scenes[index].path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath)
                                                .Replace(" ", "_")
                                                .ToUpper();
            enumContent += "\t\t" + sceneName + ",\n";
        }
        enumContent += "\t}\n}";

        // Write the enum content to a file
        System.IO.File.WriteAllText(PATH, enumContent);
        AssetDatabase.Refresh(); // Refresh the AssetDatabase to load the new script

        Debug.Log("SceneID.cs updated with current scenes!");
    }
}
#endif