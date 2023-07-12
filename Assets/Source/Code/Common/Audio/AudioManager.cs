using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.Common.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        private AudioSource[] _sources;
        private bool _soundOn = true;
        private const int AdditionalSourcesNumber = 4;

        public bool IsLoaded { get; private set; } = false;
        public bool IsSoundOn => _soundOn;

        public AudioManager Load()
        {
            if (IsLoaded) return this;

            _sources = new AudioSource[sounds.Length + AdditionalSourcesNumber];
            for (int i = 0; i < _sources.Length; i++)
            {
                _sources[i] = gameObject.AddComponent<AudioSource>();
            }
            
            IsLoaded = true;
            return this;
        }


        public AudioManager SoundOn(bool val)
        {
            _soundOn = val;
            return this;
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
            if (!_soundOn) return;
            
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
            }
        }


        public void StopAll()
        {
            foreach (var audioSource in _sources)
            {
                if (audioSource.isPlaying) audioSource.Stop();
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
