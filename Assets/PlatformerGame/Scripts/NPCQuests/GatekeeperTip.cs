using UnityEngine;

namespace Platformer
{
    public class GatekeeperTip : MonoBehaviour
    {
        [SerializeField]
        private Transform MessageTransform;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private MessageCanvas Message = null;
        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase = 0;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();
            
            //HelpText.text = Localization.Text(ETexts.Talk);
            //HelpText.gameObject.SetActive(false);

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
                    ShowMessage(Localization.Text(ETexts.Talk));
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Gatekeeper1;
                    HideMessage();
                }
            }
            
            if (quest >= 1)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    Player.Interaction += Gatekeeper2;
                    ShowMessage(Localization.Text(ETexts.Talk));
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= Gatekeeper2;
                    HideMessage();
                }
            }
        }

        private void ShowMessage(string text)
        {
            if (Message == null)
            {
                var instance = ResourceManager.GetFromPool(EComponents.MessageCanvas);
                Message = instance.GetComponent<MessageCanvas>();
                Message.SetPosition(MessageTransform.position);
                Message.SetMessage(text);
                Message.SetBlinking(true, 0.5f);
            }
        }

        private void HideMessage()
        {
            if (Message != null)
            {
                Message.gameObject.SetActive(false);
                Message = null;
            }
        }

        private void Gatekeeper1()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Gatekeeper1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
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
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.GatekeeperTitle));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Gatekeeper1_2));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    HideMessage();
                    Player.Interaction -= Gatekeeper2;
                    break;
            }
        }
    }
}