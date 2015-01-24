using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Drink : MonoBehaviour
{

    public Image BeerImage;
    public RectTransform BeerHead;

    public int FullHeadOffset = 74;
    public int EmptyHeadOffset = -120;

    [SerializeField, Range(0, 1)] private float _level;

    public float Level
    {
        get { return _level; }
        set { _level = value;
            SyncSprites();
        }
    }

    public void Update()
    {
        SyncSprites();
    }

    public void SyncSprites()
    {
        BeerImage.fillAmount = _level;
        BeerHead.localPosition = new Vector3(0, Mathf.Lerp(EmptyHeadOffset, FullHeadOffset, _level), 0);
    }
}
