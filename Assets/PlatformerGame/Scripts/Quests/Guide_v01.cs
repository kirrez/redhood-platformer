using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Guide_v01 : BaseQuest
    {
        [SerializeField]
        private GameObject Character;

        [SerializeField]
        private FarmerKnifeItemSpawner KnifeSpawner;

        [SerializeField]
        private SharpenedAxeItemSpawner AxeSpawner;

        [SerializeField]
        private KeySpawner KeySpawner;

        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.Guide_v01) > 0) return;

            if (Trigger.bounds.Contains(Player.Position) == true && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                // instant beginning
                OnInteraction();
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
            }
        }

        private void OnInteraction()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.Guide_v01_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Guide_v01_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Guide_v01_2));
                    DialoguePhase++;
                    ProgressManager.SetQuest(EQuest.FarmerKnifeItem, 1);
                    ProgressManager.SetQuest(EQuest.SharpenedAxeItem, 1);
                    KnifeSpawner.SpawnItem();
                    AxeSpawner.SpawnItem();

                    //just in case you forget about key
                    ProgressManager.SetQuest(EQuest.Blacksmith, 2);
                    ProgressManager.SetQuest(EQuest.KeyGrey, 0);
                    KeySpawner.SpawnItem();

                    break;
                case 2:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Guide_v01_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Player.ReleasedByInteraction();
                    DialoguePhase = 0;
                    Game.Dialogue.Hide();
                    HideMessage();
                    Inside = false;
                    ProgressManager.SetQuest(EQuest.Guide_v01, 1);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}

