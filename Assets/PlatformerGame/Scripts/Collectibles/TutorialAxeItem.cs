using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TutorialAxeItem : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private Rigidbody2D Rigidbody;
        private IPlayer Player;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            Player = CompositionRoot.GetPlayer();

            PhysicsOn(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var axeLevel = ProgressManager.GetQuest(EQuest.AxeLevel);

                if (axeLevel == 0)
                {
                    ProgressManager.SetQuest(EQuest.AxeLevel, 1);
                    ProgressManager.SetQuest(EQuest.TutorialAxeItem, 2); // don't want to spawn it anymore

                    Player.UpdateAllWeaponLevel();

                    Game.HUD.UpdateWeaponIcons();
                    AudioManager.PlaySound(ESounds.Collect1QuestItems); // tutorial
                }

                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}