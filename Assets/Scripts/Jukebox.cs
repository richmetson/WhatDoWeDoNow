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
            MusicPlayer.time = Random.Range(10f, MusicPlayer.clip.length/2f);
            CancelInvoke();
            Invoke("OnTrackFinished", MusicPlayer.clip.length - MusicPlayer.time);
        }

        public UnityEvent OnMusicChanged;

        public void PlayNewMusic()
        {
            CancelInvoke();
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
            Invoke("OnTrackFinished", MusicPlayer.clip.length);
        }

        private void OnTrackFinished()
        {
            PlayNewMusic();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayNewMusic();
        }
    }

}