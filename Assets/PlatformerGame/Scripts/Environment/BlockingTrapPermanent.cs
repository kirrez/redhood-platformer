using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BlockingTrapPermanent : MonoBehaviour
    {
        [SerializeField]
        private GameObject Block;

        private bool IsTriggered;
        private EQuest Index = EQuest.Cave13Block;

        private IAudioManager AudioManager;
        private IProgressManager ProgressManager;

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnEnable()
        {
            SetBlock();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!IsTriggered)
                {
                    ProgressManager.SetQuest(Index, 1);
                    SetBlock();
                    AudioManager.PlaySound(ESounds.DoorHeavy);
                }
            }
        }

        private void SetBlock()
        {
            var block = ProgressManager.GetQuest(Index);

            if (block == 0)
            {
                Block.SetActive(false);
                IsTriggered = false;
            }
            if (block == 1)
            {
                Block.SetActive(true);
                IsTriggered = true;
            }
        }
    }
}