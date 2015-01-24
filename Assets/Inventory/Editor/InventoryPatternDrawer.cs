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
            return RowHeight*(1 + property.FindPropertyRelative("Height").intValue);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var widthProp = property.FindPropertyRelative("Width");
            var heightProp = property.FindPropertyRelative("Height");
            var cellsProp = property.FindPropertyRelative("Pattern");

            EditorGUI.BeginChangeCheck();
            int width = EditorGUI.IntField(new Rect(position.xMin, position.yMin, position.width/2f, RowHeight), "Width",
                widthProp.intValue);
            int height =
                EditorGUI.IntField(
                    new Rect(position.xMin + position.width/2f, position.yMin, position.width/2f, RowHeight), "Height",
                    heightProp.intValue);
            if (EditorGUI.EndChangeCheck() || cellsProp.arraySize != (width*height))
            {
                widthProp.intValue = width;
                heightProp.intValue = height;
                cellsProp.arraySize = width * height;
            }

            var cellsRect = new Rect(position.xMin, position.yMin + RowHeight, position.width,
                position.height - RowHeight);
            var widthPerCell = 17f;
            var heightPerCell = cellsRect.height/height;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    var rect = new Rect(cellsRect.xMin + widthPerCell*x, cellsRect.yMin + heightPerCell*y,
                        widthPerCell,
                        heightPerCell);
                    EditorGUI.PropertyField(rect, cellsProp.GetArrayElementAtIndex(y*width + x),
                        GUIContent.none);
                }
            }
        }
    }

}