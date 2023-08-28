using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class HealthUpgradeQuest : MonoBehaviour
    {
        [SerializeField]
        private UpgradeHealthSpawner Spawner;

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

            AreaTrigger = GetComponent<Collider2D>();
            HelpText.text = Localization.Text(ETexts.UpgradeHealthTip);
            HelpText.gameObject.SetActive(false);
        }

        private void Update()
        {
            var pieQuest = ProgressManager.GetQuest(EQuest.MotherPie);
            var healthQuest = ProgressManager.GetQuest(EQuest.UpgradeHealth);
            
            if ( pieQuest < 5) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                if (pieQuest >= 5 && healthQuest == 0)
                {
                    Player.Interaction += OnFirstInteraction;
                }

                if (pieQuest >= 5 && healthQuest > 0)
                {
                    Player.Interaction += OnInteraction;
                }
                
                HelpText.gameObject.SetActive(true);
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                if (pieQuest >= 5 && healthQuest == 0)
                {
                    Player.Interaction -= OnFirstInteraction;
                }

                if (pieQuest >= 5 && healthQuest > 0)
                {
                    Player.Interaction -= OnInteraction;
                }

                HelpText.gameObject.SetActive(false);
            }
        }

        private void OnFirstInteraction()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.Use();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.UpgradeHealthTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.UpgradeHealth1_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth1_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth1_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth1_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Player.Idle();
                    DialoguePhase = 0;
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.UpgradeHealth, 2);
                    Player.Interaction -= OnFirstInteraction;
                    Player.Interaction += OnInteraction;
                    break;
            }
        }

        private void OnInteraction()
        {
            var Game = CompositionRoot.GetGame();
            var foodAmount = ProgressManager.GetQuest(EQuest.FoodCollected);
            var upgradeCost = ProgressManager.GetQuest(EQuest.LifeUpgradeCost);

            switch (DialoguePhase)
            {
                case 0:
                    Player.Use();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.UpgradeHealthTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth2_1));

                    if (foodAmount >= upgradeCost)
                    {
                        DialoguePhase = 5;
                    }

                    if (foodAmount < upgradeCost)
                    {
                        DialoguePhase = 10;
                    }
                    break;
                // Enough candies :
                case 5:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth2_2));
                    DialoguePhase = 6;
                    break;
                case 6:
                    Player.Idle();
                    DialoguePhase = 0;
                    Game.Dialogue.Hide();
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;

                    var foodRest = foodAmount - upgradeCost;
                    ProgressManager.SetQuest(EQuest.FoodCollected, foodRest);
                    Game.HUD.UpdateResourceAmount();

                    ProgressManager.SetQuest(EQuest.UpgradeHealth, 1); //reset value to 1 for spawner
                    Spawner.SpawnItem();
                    break;
                
                // Insufficient amount :
                case 10:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.UpgradeHealth2_3));
                    DialoguePhase = 11;
                    break;
                case 11:
                    Player.Idle();
                    DialoguePhase = 0;
                    Game.Dialogue.Hide();
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}