using UnityEditor;
using UnityEngine;
using Utils;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Disable editing of the field in the Inspector
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // Re-enable editing for other fields
    }
}