using System.CodeDom;
using UnityEditor;
using UnityEngine;

namespace AgonyBartender.Inventory.Editor
{

    [CustomPropertyDrawer(typeof (InventoryPattern))]
    public class InventoryPatternDrawer : PropertyDrawer
    {
        public const float RowHeight = 17f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded) return RowHeight;

            return RowHeight*(2 + property.FindPropertyRelative("Height").intValue);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                position.yMin += RowHeight;
                position = EditorGUI.IndentedRect(position);
                var widthProp = property.FindPropertyRelative("Width");
                var heightProp = property.FindPropertyRelative("Height");
                var cellsProp = property.FindPropertyRelative("Pattern");

                EditorGUI.PropertyField(new Rect(position.xMin, position.yMin, position.width/2f, RowHeight), widthProp);
                EditorGUI.PropertyField(
                    new Rect(position.xMin + position.width/2f, position.yMin, position.width/2f, RowHeight), heightProp);

                cellsProp.arraySize = widthProp.intValue*heightProp.intValue;

                var cellsRect = new Rect(position.xMin, position.yMin + RowHeight, position.width,
                    position.height - RowHeight);
                var widthPerCell = 17f;
                var heightPerCell = cellsRect.height/heightProp.intValue;

                for (int y = 0; y < heightProp.intValue; ++y)
                {
                    for (int x = 0; x < widthProp.intValue; ++x)
                    {
                        var rect = new Rect(cellsRect.xMin + widthPerCell*x, cellsRect.yMin + heightPerCell*y,
                            widthPerCell,
                            heightPerCell);
                        EditorGUI.PropertyField(rect, cellsProp.GetArrayElementAtIndex(y*widthProp.intValue + x),
                            GUIContent.none);
                    }
                }

                EditorGUI.indentLevel--;
            }
        }
    }

}