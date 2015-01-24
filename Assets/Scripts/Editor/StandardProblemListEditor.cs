using System;
using UnityEditor;
using UnityEngine;

namespace AgonyBartender.Editor
{
    [CustomEditor(typeof(StandardProblemList))]
    public class StandardProblemListEditor : UnityEditor.Editor
    {

        [MenuItem("Assets/Create/StandardProblemList")]
        public static void CreateAsset()
        {
            ProjectWindowUtil.CreateAsset(CreateInstance<StandardProblemList>(), "New StandardProblemList.asset");
        }

        private SerializedProperty _problemsArray;

        private void OnEnable()
        {
            _problemsArray = serializedObject.FindProperty("GlobalProblems");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_problemsArray, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
