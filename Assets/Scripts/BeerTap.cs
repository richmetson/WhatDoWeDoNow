using UnityEngine;

namespace AgonyBartender
{

    public class BeerTap : MonoBehaviour
    {
        public static BeerTap Default { get; private set; }
        public bool IsPouring { get; private set; }

        public void OnEnable()
        {
            Default = this;
        }

        public AudioSource Source;
        public AudioSource LoopSource;
        public AudioClip BeginPourClip;
        public AudioClip EndPourClip;

        public void BeginPour()
        {
            Source.clip = BeginPourClip;
            Source.Play();
            LoopSource.PlayDelayed(BeginPourClip.length);
            IsPouring = true;
        }

        public void EndPour()
        {
            IsPouring = false;
            LoopSource.Stop();
            Source.clip = EndPourClip;
            Source.Play();
        }
    }

}