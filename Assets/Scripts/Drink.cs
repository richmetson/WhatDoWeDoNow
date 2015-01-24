using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

    public float DrinkStrength = 0.1f;

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

        public bool IsEmpty
        {
            get
            {
                return Level == 0.0f;
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

        public bool IsBeingFilled { get; private set; }
        public bool IsBeingDrunk { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsBeingFilled = true;
            StartCoroutine(FillGlass());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsBeingFilled = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsBeingFilled = false;
        }

        [Serializable]
        public class BeerDispensedEvent : UnityEvent<float> { }

        public BeerDispensedEvent OnBeerDispensed;

        private IEnumerator FillGlass()
        {
            BeerTap.Default.BeginPour();
            while (IsBeingFilled)
            {
                if (Level >= 1f) break;

                var beerDispensed = Time.deltaTime*TopUpRate;
                Level += beerDispensed;
                OnBeerDispensed.Invoke(beerDispensed);
                            
                yield return null;
            }
            IsBeingFilled = false;
            BeerTap.Default.EndPour();
        }
    }

}