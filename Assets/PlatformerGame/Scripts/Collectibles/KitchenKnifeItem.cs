using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class KitchenKnifeItem : MonoBehaviour
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
                var knifeLevel = ProgressManager.GetQuest(EQuest.KnifeLevel);

                var game = CompositionRoot.GetGame();

                if (knifeLevel == 0)
                {
                    ProgressManager.SetQuest(EQuest.KnifeLevel, 1);
                    ProgressManager.SetQuest(EQuest.KitchenKnifeItem, 2); // don't want to spawn it anymore

                    Player.UpdateAllWeaponLevel();

                    game.HUD.UpdateWeaponIcons();
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