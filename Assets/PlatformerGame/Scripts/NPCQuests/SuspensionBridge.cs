using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SuspensionBridge : MonoBehaviour
    {
        [SerializeField]
        private GameObject Bridge;

        [SerializeField]
        private GameObject LooseRopes;

        [SerializeField]
        private GameObject Shatters;

        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            if (ProgressManager.GetQuest(EQuest.SuspensionBridge) != 1)
            {
                Bridge.SetActive(true);
                LooseRopes.SetActive(false);
            }

            if (ProgressManager.GetQuest(EQuest.SuspensionBridge) == 1)
            {
                Bridge.SetActive(false);
                LooseRopes.SetActive(true);
            }

            Shatters.SetActive(false);
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.SuspensionBridge) == 2)
            {
                ProgressManager.SetQuest(EQuest.SuspensionBridge, 3);
                Bridge.SetActive(true);
                LooseRopes.SetActive(false);
            }


        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.SuspensionBridge) > 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(EQuest.SuspensionBridge, 1);
                Bridge.SetActive(false);
                LooseRopes.SetActive(true);
                Shatters.SetActive(true);

                //In-quest SpawnPoint update!
                ProgressManager.SetQuest(EQuest.SpawnPoint, 1);


                //Player.HoldByInteraction();
            }
        }
    }
}