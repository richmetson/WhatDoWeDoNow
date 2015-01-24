using UnityEditor;

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
	}
}
