using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(BuildingsManifestSO))]
public class BuildingPropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        // Create drawer UI using C#
        var popup = new UnityEngine.UIElements.PopupWindow();
        popup.text = "Tire Details";
        popup.Add(new PropertyField(property.FindPropertyRelative("ID"), "ID"));
        popup.Add(new PropertyField(property.FindPropertyRelative("building"), "Building"));
        container.Add(popup);

        return container;
    }
}
