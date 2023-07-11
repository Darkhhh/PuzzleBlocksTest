using System;
using UnityEngine;

namespace Source.Code.Common.Audio
{
    [Serializable]
    public class Sound
    {
        public SoundTag tag;
        
        public AudioClip clip;

        [Range(0f, 1f)] public float volume;

        [Range(0.1f, 3f)] public float pitch;

        public bool loop;

        [HideInInspector] public AudioSource source;


        public void InitializeAudioSource()
        {
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;

            source.clip = clip;
        }
    }
}