#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class LayerEnumGenerator : MonoBehaviour
    {
        private const string PATH = "Assets/Project/Modules/Utils/Scripts/Layers.cs";

        // Menu item to manually regenerate the enum file
        [MenuItem("Tools/Update Layers Enum")]
        public static void GenerateLayersEnum()
        {
            // Access the TagManager via reflection
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");

            // Build the enum content
            string enumContent = "namespace Utils\n{\n\tpublic enum Layers \n\t{\n";

            for (int index = 0; index < layersProp.arraySize; index++)
            {
                SerializedProperty t = layersProp.GetArrayElementAtIndex(index);
                string layer = t.stringValue;

                if (layer == string.Empty) continue; // Skip empty layers

                // Create a valid enum entry (sanitize to avoid spaces and invalid characters)
                string sanitizedTag = layer.Replace(" ", "_").Replace("-", "_");
                enumContent += "\t\t" + sanitizedTag + ",\n";
            }
            enumContent += "\t}\n}";

            // Write the enum content to a file
            File.WriteAllText(PATH, enumContent);
            AssetDatabase.Refresh(); // Refresh the AssetDatabase to load the new script

            Debug.Log("Tags.cs updated with current layers!");
        }
    }
}
#endif