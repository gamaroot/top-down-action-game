using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class TagEnumGenerator : MonoBehaviour
    {
        private const string PATH = "Assets/Project/Modules/Utils/Scripts/Tags.cs";

        private static readonly string[] INTERNAL_TAGS = new string[]
        {
            "Untagged",
            "Respawn",
            "Finish",
            "EditorOnly",
            "MainCamera",
            "Player",
            "GameController",
        };

        // Menu item to manually regenerate the enum file
        [MenuItem("Tools/Update Tags Enum")]
        public static void GenerateTagEnum()
        {
            // Access the TagManager via reflection
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Build the enum content
            string enumContent = "namespace Utils\n{\n\tpublic enum Tags \n\t{\n";

            // Adding Unity internal tags
            for (int index = 0; index < INTERNAL_TAGS.Length; index++)
            {
                string tag = INTERNAL_TAGS[index];
                enumContent += "\t\t" + tag + ",\n";
            }

            for (int index = 0; index < tagsProp.arraySize; index++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(index);
                string tag = t.stringValue;

                // Create a valid enum entry (sanitize to avoid spaces and invalid characters)
                string sanitizedTag = tag.Replace(" ", "_").Replace("-", "_");
                enumContent += "\t\t" + sanitizedTag + ",\n";
            }
            enumContent += "\t}\n}";

            // Write the enum content to a file
            File.WriteAllText(PATH, enumContent);
            AssetDatabase.Refresh(); // Refresh the AssetDatabase to load the new script

            Debug.Log("Tags.cs updated with current tags!");
        }
    }
}
