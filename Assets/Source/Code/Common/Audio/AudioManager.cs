using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.Common.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        private AudioSource[] _sources;
        private const int AdditionalSourcesNumber = 4;

        public bool IsLoaded { get; private set; } = false;

        public void Load()
        {
            if (IsLoaded) return;

            _sources = new AudioSource[sounds.Length + AdditionalSourcesNumber];
            for (int i = 0; i < _sources.Length; i++)
            {
                _sources[i] = gameObject.AddComponent<AudioSource>();
            }
            // foreach (var sound in sounds)
            // {
            //     sound.source = gameObject.AddComponent<AudioSource>();
            //     sound.InitializeAudioSource();
            // }
            
            IsLoaded = true;
        }


        public void LoadAndPlay(SoundTag soundTag)
        {
            if (IsLoaded)
            {
                Play(soundTag);
                return;
            }
            
            Load();
            Play(soundTag);
        }


        public void Play(SoundTag soundTag)
        {
            foreach (var s in sounds)
            {
                if(s.tag != soundTag) continue;

                foreach (var source in _sources)
                {
                    if (source.isPlaying) continue;

                    s.source = source;
                    s.InitializeAudioSource();
                    s.source.Play();
                    return;
                }
                // if (s.source.isPlaying) return;
                // s.source.Play();
                // return;
            }
        }


        public void PlayWithDelay(SoundTag soundTag, float delay)
        {
            foreach (var s in sounds)
            {
                if(s.tag != soundTag) continue;
                
                s.source.PlayDelayed(delay);
                return;
            }
        }


        public void Stop(SoundTag soundTag)
        {
            foreach (var s in sounds)
            {
                if(s.tag != soundTag) continue;
                
                s.source.Stop();
                return;
            }
        }
    }
}
