using System.Collections;
using Nidavellir.Scriptables.Audio;
using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir.Audio
{
    /// <summary>
    /// This Component can be referenced in any other component which might play any Sfx. Be it a looping sound or just playing a Sfx oneshot.
    /// </summary>
    public class SfxPlayer : MonoBehaviour
    {
        private AudioSource m_loopingAudioSource;

        private AudioSource m_tmpAudioSource;
        
        public void PlayLoopingSfx(SfxData sfxData)
        {
            if (this.m_loopingAudioSource == null)
            {
                this.m_loopingAudioSource = this.AddComponent<AudioSource>();
                this.m_loopingAudioSource.loop = true;
            }

            this.PlayClipOnAudioSource(sfxData, this.m_loopingAudioSource);
        }

        public void StopLoopingCurrentSfx()
        {
            this.m_loopingAudioSource.Stop();
        }

        public void StopPlaying()
        {
            if (this.m_tmpAudioSource is null)
                return;
            
            this.m_tmpAudioSource.Stop();
        }
        
        public void PlayOneShot(SfxData sfxData)
        {
            this.StartCoroutine(this.PlayClipAndDestroySource(sfxData));
        }

        private IEnumerator PlayClipAndDestroySource(SfxData data)
        {
            this.m_tmpAudioSource = this.AddComponent<AudioSource>();
            this.PlayClipOnAudioSource(data, this.m_tmpAudioSource);
            yield return new WaitForSeconds(data.AudioClip.length);
            Destroy(this.m_tmpAudioSource);
            this.m_tmpAudioSource = null;
        }

        private void PlayClipOnAudioSource(SfxData sfxData, AudioSource audioSource)
        {
            audioSource.clip = sfxData.AudioClip;
            audioSource.volume = sfxData.Volume * GlobalSettings.Instance.SfxVolume;
            audioSource.Play();
        }
    }
}