using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

namespace Platformer
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private Dictionary<EPlayerSounds, AudioSource> PlayerSounds = new Dictionary<EPlayerSounds, AudioSource>();

        private AudioSource CurrentMusic;

        private EMusic MusicIndex;


        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();

            LoadPlayerSounds();
        }

        private AudioSource LoadSound(ESounds sound)
        {
            var instance = ResourceManager.GetFromPool(sound);
            DynamicsContainer.AddSound(instance);
            return instance.GetComponent<AudioSource>();
        }

        private void LoadPlayerSounds()
        {
            PlayerSounds.Add(EPlayerSounds.ThrowKnife, LoadSound(ESounds.ThrowKnife1));
            PlayerSounds.Add(EPlayerSounds.ThrowAxe, LoadSound(ESounds.ThrowAxe1));
            PlayerSounds.Add(EPlayerSounds.ThrowBottle, LoadSound(ESounds.ThrowKnife2));

            PlayerSounds.Add(EPlayerSounds.Jump, LoadSound(ESounds.PlayerJump));
            PlayerSounds.Add(EPlayerSounds.Landing, LoadSound(ESounds.PlayerLanding));
            PlayerSounds.Add(EPlayerSounds.RollDown, LoadSound(ESounds.RollDown2));
            PlayerSounds.Add(EPlayerSounds.WaterSplash, LoadSound(ESounds.Splash1));

            PlayerSounds.Add(EPlayerSounds.DamageTaken, LoadSound(ESounds.MCDamage3));
            PlayerSounds.Add(EPlayerSounds.LifeLost, LoadSound(ESounds.LifeLost1));
            PlayerSounds.Add(EPlayerSounds.LightCampFire, LoadSound(ESounds.Collect7CampFire));
        }

        public void PlayRedhoodSound(EPlayerSounds key)
        {
            PlayerSounds[key].Play();
        }

        public void PlayMusic(EMusic index)
        {
            if (index == MusicIndex && CurrentMusic != null) return;

            if (index != MusicIndex)
            {
                CurrentMusic.Stop();
                CurrentMusic.gameObject.SetActive(false);
                MusicIndex = index;
            }

            var instance = ResourceManager.GetFromPool(index);
            DynamicsContainer.AddMusic(instance);

            CurrentMusic = instance.GetComponent<AudioSource>();
            CurrentMusic.Play();
        }

        public void PauseMusic()
        {
            if (CurrentMusic != null)
            {
                CurrentMusic.Pause();
            }
        }

        public void UnPauseMusic()
        {
            if (CurrentMusic != null)
            {
                CurrentMusic.UnPause();
            }
        }

        public void ReplayMusic()
        {
            if (CurrentMusic != null)
            {
                CurrentMusic.Stop();
                CurrentMusic.Play();
            }
        }

        public void PlaySound(ESounds sound, ContainerSort sort = ContainerSort.Sounds)
        {
            var instance = ResourceManager.GetFromPool(sound);

            if (sort == ContainerSort.Temporary)
            {
                DynamicsContainer.AddTemporary(instance);
            }
            else
            {
                DynamicsContainer.AddSound(instance);
            }

            instance.GetComponent<AudioSource>().Play();
            var breaker = instance.GetComponent<SoundBreaker>();
            breaker.StartCountDown();
        }
    }
}