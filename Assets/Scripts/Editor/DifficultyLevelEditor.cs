using UnityEditor;

namespace AgonyBartender.Editor
{

    [CustomEditor(typeof(DifficultyLevel))]
    public class DifficultyLevelEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/Difficulty Level")]
        public static void CreateAsset()
        {
            ProjectWindowUtil.CreateAsset(CreateInstance<DifficultyLevel>(), "New Difficulty Level.asset");
        }
    }

}