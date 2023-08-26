using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class CommonerSecret : MonoBehaviour
    {
        [SerializeField]
        Text HelpText;

        private ILocalization Localization;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase = 0;

        private void Awake()
        {
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            HelpText.text = Localization.Text(ETexts.Talk);
            HelpText.gameObject.SetActive(false);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.CommonerSecret));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}