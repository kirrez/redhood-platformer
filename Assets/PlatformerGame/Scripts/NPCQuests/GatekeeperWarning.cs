using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class GatekeeperWarning : MonoBehaviour
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
                    Player.Use();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Commoner1_2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.Idle();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;
                    break;
            }
        }
    }
}