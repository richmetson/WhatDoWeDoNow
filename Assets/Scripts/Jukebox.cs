using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AgonyBartender
{

    public class Jukebox : MonoBehaviour, IPointerClickHandler
    {
        public AudioSource MusicPlayer;

        public int CurrentMusicIndex = -1;
        public AudioClip[] MusicList;

        public void Start()
        {
            PlayNewMusic();
        }

        public UnityEvent OnMusicChanged;

        public void PlayNewMusic()
        {
            if (MusicList.Length > 1)
            {
                int newValue = CurrentMusicIndex;
                while (newValue == CurrentMusicIndex)
                    newValue = Random.Range(0, MusicList.Length);
                CurrentMusicIndex = newValue;
            }
            else CurrentMusicIndex = 0;

            MusicPlayer.Stop();
            MusicPlayer.clip = MusicList[CurrentMusicIndex];
            MusicPlayer.Play();
            OnMusicChanged.Invoke();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            PlayNewMusic();
        }
    }

}