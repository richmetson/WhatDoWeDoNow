using UnityEditor;

namespace AgonyBartender.Editor
{
    [CustomEditor(typeof(OverheardConversation))]
    public class OverheardConversationEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/OverheardConversation")]
        public static void CreateAsset()
        {
            ProjectWindowUtil.CreateAsset(CreateInstance<OverheardConversation>(), "New OverheardConversation.asset");
        }
    }
}