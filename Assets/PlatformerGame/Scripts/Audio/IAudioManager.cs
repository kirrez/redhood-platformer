using UnityEngine;

namespace Platformer
{
    public interface IAudioManager
    {
        void PlayMusic(EMusic index);
        void PauseMusic(float delay = 0f, bool replay = false);
        void UnPauseMusic();
        void ReplayMusic();
        void PlaySound(ESounds sound, ContainerSort sort = ContainerSort.Sounds);

        void PlayRedhoodSound(EPlayerSounds sound);
    }
}