using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AgonyBartender.Editor
{

    [CustomEditor(typeof(Problem))]
    public class ProblemEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/Problem")]
        public static void CreateAsset()
        {
            ProjectWindowUtil.CreateAsset(CreateInstance<Problem>(), "New Problem.asset");
        }
    }

}