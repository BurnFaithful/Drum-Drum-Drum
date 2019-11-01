using UnityEngine;
using UnityEditor;

namespace CustomAttribute
{

    [CustomPropertyDrawer(typeof(AliasAttribute))]
    internal class AliasAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AliasAttribute aliasAttribute = (AliasAttribute)attribute;
            label.text = aliasAttribute.Alias;
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}