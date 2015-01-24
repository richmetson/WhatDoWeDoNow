using System;
using UnityEditor;
using UnityEngine;

namespace AgonyBartender.Editor
{
	[CustomEditor(typeof (Patron))]
	public class PatronEditor : UnityEditor.Editor
	{
		[MenuItem("Assets/Create/Patron")]
		public static void CreateAsset()
		{
			ProjectWindowUtil.CreateAsset(CreateInstance<Patron>(), "New Patron.asset");
		}

	    private SerializedProperty _patronName;
	    private SerializedProperty _faceSpritesArray;
	    private SerializedProperty _problemsArray;

	    private ProblemSolutionFacialExpression[] _faceExpressions;

	    private void OnEnable()
	    {
	        _patronName = serializedObject.FindProperty("PatronName");
	        _faceSpritesArray = serializedObject.FindProperty("FaceSprites");
	        _problemsArray = serializedObject.FindProperty("PatronsProblems");

	        _faceExpressions = (ProblemSolutionFacialExpression[])Enum.GetValues(typeof (ProblemSolutionFacialExpression));
	        _faceSpritesArray.arraySize = _faceExpressions.Length;
	    }

	    public override void OnInspectorGUI()
	    {
            serializedObject.Update();
	        EditorGUILayout.PropertyField(_patronName);

	        for (int i = 0; i < _faceSpritesArray.arraySize; ++i)
	        {
	            var expr = _faceSpritesArray.GetArrayElementAtIndex(i);
	            EditorGUILayout.PropertyField(expr, new GUIContent(_faceExpressions[i].ToString()));
	        }

	        EditorGUILayout.PropertyField(_problemsArray, true);
	        serializedObject.ApplyModifiedProperties();
	    }
	}
}
