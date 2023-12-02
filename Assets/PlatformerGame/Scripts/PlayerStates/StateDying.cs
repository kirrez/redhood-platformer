using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerStates
{
    public class StateDying : BaseState
    {
        private IAudioManager AudioManager;

        public StateDying(IPlayer model)
        {
            Model = model;
            AudioManager = CompositionRoot.GetAudioManager();
        }

        public override void OnEnable(float time = 0f)
        {
            Timer = time;
            Model.UpdateStateName("Dying");
            Model.Health.HealthChanged = null;

            AudioManager.PauseMusic();
            AudioManager.PlayRedhoodSound(EPlayerSounds.LifeLost);
        }

        public override void Update()
        {
            // no player control input
        }

        public override void FixedUpdate()
        {
            // no base

            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Model.EnableGameOver();
            }
        }

    }
}