using Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;


namespace Scripts.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioSource _VOSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioClip[] _VO;
        [SerializeField] private AudioClip[] _music;
        [SerializeField] private AudioClip[] _sfx;

        private float _audioLength;
        private bool _isMusic;


        public static Action<int> onMusicEnd;
        public static Action<int> onVOEnd;

        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            GameSceneManager.onSendMusic += ReceiveMusic;
            GameSceneManager.onSendVO += ReceiveVO;
            PoolManager.onBrewCoffeeSFX += ReceiveSFX;
            CoffeeSphere.onCoffeeDrank += ReceiveSFX;
        }


        public void ReceiveSFX(int clipID)
        {
            PlaySFX(clipID, _sfxSource.volume);
        }

        public void ReceiveMusic(int clipID)
        {
            _isMusic = true;
            StartCoroutine(AudioRoutine(clipID, _musicSource.volume));
        }

        public void ReceiveVO(int clipID)
        {
            _isMusic = false;
            StartCoroutine(AudioRoutine(clipID, _VOSource.volume));
        }

        public void PlaySFX(int clipID, float volume)
        {
            _sfxSource.PlayOneShot(_sfx[clipID], volume);
        }

        IEnumerator AudioRoutine(int clipID, float volume)
        {
            if (_isMusic == true)
            {
                _musicSource.PlayOneShot(_music[clipID], volume);
                _audioLength = _music[clipID].length;
            }
            else
            {
                _VOSource.PlayOneShot(_VO[clipID], volume);
                _audioLength = _VO[clipID].length;
            }

            yield return new WaitForSeconds(_audioLength);

            if (_isMusic == true)
            {
                onMusicEnd(clipID);
            }
            else
            {
                onVOEnd(clipID);
            }
        }

        private void OnDisable()
        {
            GameSceneManager.onSendMusic -= ReceiveMusic;
            GameSceneManager.onSendVO -= ReceiveVO;
            PoolManager.onBrewCoffeeSFX -= ReceiveSFX;
            CoffeeSphere.onCoffeeDrank -= ReceiveSFX;
        }
    }
}

