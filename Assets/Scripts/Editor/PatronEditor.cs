﻿using System;
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

        private SerializedProperty _gapBetweenGulps;
        private SerializedProperty _lengthOfGulp;
        private SerializedProperty _gulpMagnitude;
        private SerializedProperty _alcoholIntolerance;

        private void OnEnable()
        {
            _patronName = serializedObject.FindProperty("PatronName");
            _faceSpritesArray = serializedObject.FindProperty("FaceSprites");
            _problemsArray = serializedObject.FindProperty("PatronsProblems");

            _faceExpressions = (ProblemSolutionFacialExpression[])Enum.GetValues(typeof(ProblemSolutionFacialExpression));
            _faceSpritesArray.arraySize = _faceExpressions.Length;

            _gapBetweenGulps = serializedObject.FindProperty("GapBetweenGulps");
            _lengthOfGulp = serializedObject.FindProperty("LengthOfGulp");
            _gulpMagnitude = serializedObject.FindProperty("GulpMagnitude");
            _alcoholIntolerance = serializedObject.FindProperty("AlcoholIntolerance");
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

            EditorGUILayout.PropertyField(_gapBetweenGulps, true);
            EditorGUILayout.PropertyField(_lengthOfGulp, true);
            EditorGUILayout.PropertyField(_gulpMagnitude, true);
            EditorGUILayout.PropertyField(_alcoholIntolerance, true);

	        serializedObject.ApplyModifiedProperties();
	    }
	}
}
