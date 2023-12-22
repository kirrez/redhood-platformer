using UnityEngine;

// Changes Mother's pie value from 0 to 1, spawns Red key in cellar

namespace Platformer
{
    public class MotherQuest1 : MonoBehaviour
    {
        [SerializeField]
        private KeySpawner Spawner;

        [SerializeField]
        private Transform MessageTransform;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

        private MessageCanvas Message = null;
        private Collider2D AreaTrigger;
        private bool Inside = false;
        private int DialoguePhase;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Player = CompositionRoot.GetPlayer();

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void Update()
        {
            int quest = ProgressManager.GetQuest(EQuest.MotherPie);
            //if (quest != 0) return;

            if (quest == 0)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    DialoguePhase = 0;
                    Player.Interaction += MQ1First;

                    ShowMessage(Localization.Text(ETexts.TalkToMom));
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= MQ1First;

                    HideMessage();
                }
            }
            
            if (quest > 0 && quest < 3)
            {
                if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
                {
                    Inside = true;
                    DialoguePhase = 0;
                    Player.Interaction += MQ1Second;

                    ShowMessage(Localization.Text(ETexts.TalkToMom));
                }

                if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
                {
                    Inside = false;
                    Player.Interaction -= MQ1Second;

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

        private void MQ1First()
        {
            var Game = CompositionRoot.GetGame();

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue1));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie1_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie1_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie1_6));
                    DialoguePhase++;
                    break;
                case 6:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.MotherPie, 1);

                    HideMessage();

                    ProgressManager.SetQuest(EQuest.KeyRed, 0);
                    Spawner.SpawnItem();
                    Player.Interaction -= MQ1First;
                    break;
            }
        }

        private void MQ1Second()
        {
            var Game = CompositionRoot.GetGame();

            var schrooms = ProgressManager.GetQuest(EQuest.MushroomsCollected);
            var berries = ProgressManager.GetQuest(EQuest.BlackberriesCollected);
            var schroomsRequired = ProgressManager.GetQuest(EQuest.MushroomsRequired);
            var berriesRequiered = ProgressManager.GetQuest(EQuest.BlackberriesRequired);

            string phrase = Localization.Text(ETexts.DialoguePie1_7) + schrooms.ToString() + "/" + schroomsRequired.ToString() + Localization.Text(ETexts.DialoguePie1_8) + berries.ToString() + "/" + berriesRequiered.ToString() + Localization.Text(ETexts.DialoguePie1_9);

            switch (DialoguePhase)
            {
                case 0:
                    Player.HoldByInteraction();
                    Message.StopBlinking();
                    Game.Dialogue.Show();
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue1));
                    Game.Dialogue.ChangeContent(phrase);
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();

                    HideMessage();

                    Player.Interaction -= MQ1Second;
                    break;
            }

        }

    }
}