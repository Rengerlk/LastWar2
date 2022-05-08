using UnityEngine;

namespace PixelCrew.Utils
{
    public class AudioUtils
    {
        public const string SfxAudioSource = "SfxAudioSource";
        public static AudioSource FindSfxSource()
        {
           return GameObject.FindWithTag(SfxAudioSource).GetComponent<AudioSource>();
        }
    }
}