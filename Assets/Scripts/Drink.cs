using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender
{

    [ExecuteInEditMode]
    public class Drink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {

        public Image BeerImage;
        public RectTransform BeerHead;

        public int FullHeadOffset = 74;
        public int EmptyHeadOffset = -120;

        public float TopUpRate = 0.1f;

        [SerializeField, Range(0, 1)] private float _level;

        public float Level
        {
            get { return _level; }
            set
            {
                _level = Mathf.Clamp01(value);
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

        private bool _isFilling;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isFilling = true;
            StartCoroutine(FillGlass());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isFilling = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isFilling = false;
        }

        private IEnumerator FillGlass()
        {
            while (_isFilling)
            {
                Level += Time.deltaTime*TopUpRate;
                yield return null;
            }
        }
    }

}