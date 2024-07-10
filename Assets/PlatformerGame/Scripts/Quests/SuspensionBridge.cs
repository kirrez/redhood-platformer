using UnityEngine;

// Changes Suspension bridge's value from 0 to 1
// Changes Suspension bridge's value from 2 to 3

namespace Platformer
{
    public class SuspensionBridge : BaseQuest
    {
        [SerializeField]
        private GameObject Bridge;

        [SerializeField]
        private GameObject LooseRopes;

        [SerializeField]
        private GameObject Shatters;

        private IFadeScreen FadeScreen;

        protected override void Awake()
        {
            base.Awake();
            // FadeScreen
        }

        private void OnEnable()
        {
            var quest = ProgressManager.GetQuest(EQuest.SuspensionBridge);

            if (quest != 1)
            {
                Bridge.SetActive(true);
                LooseRopes.SetActive(false);
            }

            if (quest == 1)
            {
                Bridge.SetActive(false);
                LooseRopes.SetActive(true);
            }

            Shatters.SetActive(false);
        }

        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.SuspensionBridge);

            if (Trigger.bounds.Contains(Player.Position) && quest == 0)
            {
                var game = CompositionRoot.GetGame();

                ProgressManager.SetQuest(EQuest.SuspensionBridge, 1);
                Bridge.SetActive(false);
                LooseRopes.SetActive(true);
                Shatters.SetActive(true);
                Game.FadeScreen.FadeOut(new Color(1f, 1f, 1f, 1f), 1f);

                //In-quest SpawnPoint update!
                ProgressManager.SetQuest(EQuest.Location, 0);
                ProgressManager.SetQuest(EQuest.SpawnPoint, 1);
                ProgressManager.SetQuest(EQuest.Confiner, 0);

                Player.GetStunned(1.5f);

                AudioManager.PlaySound(ESounds.Blast8, ContainerSort.Temporary);
                AudioManager.PlaySound(ESounds.Fall1, ContainerSort.Temporary);
            }

            if (quest == 2)
            {
                ProgressManager.SetQuest(EQuest.SuspensionBridge, 3);
                Bridge.SetActive(true);
                LooseRopes.SetActive(false);
            }
        }
    }
}