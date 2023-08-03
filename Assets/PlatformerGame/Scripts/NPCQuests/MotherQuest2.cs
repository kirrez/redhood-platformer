using UnityEngine.UI;
using UnityEngine;

// changes Mother's pie from 3 to 4

namespace Platformer
{
    public class MotherQuest2 : MonoBehaviour
    {
        [SerializeField]
        Transform PiePosition;

        [SerializeField]
        Text HelpText;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private IPlayer Player;

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
        }

        private void OnEnable()
        {
            HelpText.text = Localization.Text(ETexts.TalkToMomAgain);
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 3) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                HelpText.gameObject.SetActive(true);

                var BerriesEnough = ProgressManager.GetQuest(EQuest.BlackberriesCollected) >= ProgressManager.GetQuest(EQuest.BlackberriesRequired);
                var MushroomsEnough = ProgressManager.GetQuest(EQuest.MushroomsCollected) >= ProgressManager.GetQuest(EQuest.MushroomsRequired);

                if (BerriesEnough && MushroomsEnough)
                {
                    Player.Interaction += OnInteraction;
                }
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
                    Game.Dialogue.SetDialogueName(Localization.Text(ETexts.PieDialogue2));
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_1));
                    DialoguePhase++;
                    break;
                case 1:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_2));
                    DialoguePhase++;
                    break;
                case 2:
                    Game.Dialogue.AddContent(Localization.Text(ETexts.DialoguePie2_3));
                    DialoguePhase++;
                    break;
                case 3:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_4));
                    DialoguePhase++;
                    break;
                case 4:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_5));
                    DialoguePhase++;
                    break;
                case 5:
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.DialoguePie2_6));
                    DialoguePhase++;
                    break;
                case 6:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    ProgressManager.SetQuest(EQuest.MotherPie, 4);
                    HelpText.gameObject.SetActive(false);
                    Player.Interaction -= OnInteraction;

                    var instance = ResourceManager.CreatePrefab<MotherPie, ECollectibles>(ECollectibles.MotherPie);
                    instance.transform.SetParent(transform, false);
                    instance.transform.position = PiePosition.position;
                    break;
            }
        }
    }
}