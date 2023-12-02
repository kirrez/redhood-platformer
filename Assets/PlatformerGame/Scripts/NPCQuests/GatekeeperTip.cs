using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class GatekeeperTip : MonoBehaviour
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
            
            HelpText.text = Localization.Text(ETexts.Talk);
            HelpText.gameObject.SetActive(false);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            int quest = ProgressManager.GetQuest(EQuest.KeyGrey);
            //if ( quest == 1) return;

            if (quest < 1)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Gatekeeper1;
                    HelpText.gameObject.SetActive(true);
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Gatekeeper1;
                    HelpText.gameObject.SetActive(false);
                }
            }
            
            if (quest >= 1)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Gatekeeper2;
                    HelpText.gameObject.SetActive(true);
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Gatekeeper2;
                    HelpText.gameObject.SetActive(false);
                }
            }
        }

        private void Gatekeeper1()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Gatekeeper1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= Gatekeeper1;
                    break;
            }
        }

        private void Gatekeeper2()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Gatekeeper1_2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= Gatekeeper2;
                    break;
            }
        }
    }
}