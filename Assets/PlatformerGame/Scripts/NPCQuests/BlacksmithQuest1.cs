using UnityEngine.UI;
using UnityEngine;

// Changes Blacksmith value from 0 to 1 and then to 2, spawns Grey key

namespace Platformer
{
    public class BlacksmithQuest1 : MonoBehaviour
    {
        [SerializeField]
        private KeySpawner Spawner;
        
        [SerializeField]
        private Text HelpText;

        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase = 0;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            HelpText.text = Localization.Text(ETexts.Talk);
            HelpText.gameObject.SetActive(false);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            var quest = ProgressManager.GetQuest(EQuest.Blacksmith);

            if (ProgressManager.GetQuest(EQuest.Blacksmith) > 1) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                if (quest == 0)
                {
                    //Thats bad practice
                    Player.Interaction += OnKeyQuestPhase0;
                }

                if (quest == 1)
                {
                    //Thats bad practice
                    Player.Interaction += OnKeyQuestPhase1;
                }
                
                HelpText.gameObject.SetActive(true);
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                if (quest == 0)
                {
                    //Thats bad practice
                    Player.Interaction -= OnKeyQuestPhase0;
                }

                if (quest == 1)
                {
                    //Thats bad practice
                    Player.Interaction -= OnKeyQuestPhase1;
                }

                HelpText.gameObject.SetActive(false);
            }
        }

        private void OnKeyQuestPhase0()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.Use();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BlacksmithTitle1));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Blacksmith1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith1_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith1_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Blacksmith1_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith1_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Player.Idle();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.Blacksmith, 1);
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnKeyQuestPhase0;
                    DialoguePhase = 0;
                    break;
            }
        }

        private void OnKeyQuestPhase1()
        {
            var Game = CompositionRoot.GetGame();
            var oreAmount = ProgressManager.GetQuest(EQuest.OreCollected);

            switch (DialoguePhase)
            {
                case 0:
                    Player.Use();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BlacksmithTitle1));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Blacksmith2_1));

                    if (oreAmount >= 3)
                    {
                        DialoguePhase = 5;
                    }

                    if (oreAmount < 3)
                    {
                        DialoguePhase = 10;
                    }
                    
                    break;
                case 5:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith2_2));
                    DialoguePhase++;
                    break;
                case 6:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith2_3));
                    DialoguePhase++;
                    break;
                case 7:
                    Player.Idle();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.Blacksmith, 2);
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnKeyQuestPhase1;

                    var oreRest = oreAmount - 3;
                    ProgressManager.SetQuest(EQuest.OreCollected, oreRest);
                    Game.HUD.UpdateResourceAmount();

                    ProgressManager.SetQuest(EQuest.KeyGrey, 0);
                    Spawner.SpawnItem();
                    DialoguePhase = 0;
                    break;

                case 10:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.Blacksmith2_4));
                    DialoguePhase++;
                    break;
                case 11:
                    Player.Idle();
                    Game.Dialogue.Hide();
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnKeyQuestPhase1;
                    DialoguePhase = 0;
                    break;
            }
        }

    }
}