using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SwitchHandleItem : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private Rigidbody2D Rigidbody;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Rigidbody = GetComponent<Rigidbody2D>();

            PhysicsOn(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var quest = ProgressManager.GetQuest(EQuest.SwitchHandleItem);

                if (quest == 0)
                {
                    ProgressManager.SetQuest(EQuest.SwitchHandleItem, 1); // don't want to spawn it anymore
                    AudioManager.PlaySound(ESounds.Collect1QuestItems);
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