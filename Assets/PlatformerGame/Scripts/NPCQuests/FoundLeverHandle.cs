using UnityEngine;

namespace Platformer
{
    public class FoundLeverHandle : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private int DialoguePhase = 0;
        private bool IsInside = false;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            var quest = ProgressManager.GetQuest(EQuest.BrokenLeverTip);

            if (quest > 0) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && IsInside == false)
            {
                IsInside = true;
                Player.Interaction += OnInteraction;
                OnInteraction();
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.BrokenLeverTip_Title));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.BrokenLeverTip1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.BrokenLeverTip, 1);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}