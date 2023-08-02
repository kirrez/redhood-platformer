using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class MarketElevatorTip : MonoBehaviour
    {
        [SerializeField]
        Text HelpText;

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
        }

        private void OnEnable()
        {
            HelpText.text = Localization.Text(ETexts.Talk);
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.MarketElevator) > 1) return; // Elevator works when value becomes "2" 

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                HelpText.gameObject.SetActive(true);
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HelpText.gameObject.SetActive(false);
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.VillageCommoner));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Commoner1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;
                    DialoguePhase = 0;
                    break;
            }

        }
    }
}