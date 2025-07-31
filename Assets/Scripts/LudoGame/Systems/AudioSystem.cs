using UnityEngine;

namespace LudoGame.Systems
{
    /// <summary>
    /// Manages game audio effects
    /// </summary>
    public class AudioSystem : MonoBehaviour, IAudioSystem
    {
        [SerializeField] private AudioClip diceRollSound;
        [SerializeField] private AudioClip tokenMoveSound;
        [SerializeField] private AudioClip tokenKillSound;
        [SerializeField] private AudioClip victorySound;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlayDiceRoll()
        {
            PlaySound(diceRollSound);
        }

        public void PlayTokenMove()
        {
            PlaySound(tokenMoveSound);
        }

        public void PlayTokenKill()
        {
            PlaySound(tokenKillSound);
        }

        public void PlayVictory()
        {
            PlaySound(victorySound);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip && _audioSource)
            {
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}
