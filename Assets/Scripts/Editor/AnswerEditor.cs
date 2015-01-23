using UnityEditor;

namespace AgonyBartender.Editor
{
    [CustomEditor(typeof (Answer))]
    public class AnswerEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/Answer")]
        public static void CreateAsset()
        {
            ProjectWindowUtil.CreateAsset(CreateInstance<Answer>(), "New Answer.asset");
        }
    }
}