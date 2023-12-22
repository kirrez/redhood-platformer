using UnityEngine;

namespace Platformer
{
    public class MarketElevatorTip : MonoBehaviour
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

            AreaTrigger = GetComponent<Collider2D>();

            //HelpText.text = Localization.Text(ETexts.Talk);
            //HelpText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.MarketElevator) > 1) return; // Elevator works when value becomes "2" 

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                ShowMessage(Localization.Text(ETexts.Talk));
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HideMessage();
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

        private void OnInteraction()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.VillageCommoner));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.Commoner1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    HideMessage();
                    Player.Interaction -= OnInteraction;
                    DialoguePhase = 0;
                    break;
            }

        }
    }
}