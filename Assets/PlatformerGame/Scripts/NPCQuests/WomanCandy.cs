using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class WomanCandy : MonoBehaviour
    {
        [SerializeField]
        private FoodSpawner Spawner;

        [SerializeField]
        private NPCWanderer Wanderer;

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

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.WomanCandy) == 1) return;

            if (AreaTrigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
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
                    Game.Dialogue.ChangeContent(Localization.Text(ETexts.WomanCandy));
                    ProgressManager.SetQuest(EQuest.WomanCandy, 1);
                    Wanderer.Stun(true);
                    DialoguePhase++;
                    break;
                case 1:
                    Player.ReleasedByInteraction();
                    Game.Dialogue.Hide();
                    DialoguePhase = 0;
                    Player.Interaction -= OnInteraction;
                    Spawner.gameObject.SetActive(true);
                    Wanderer.Stun(false);
                    break;
            }
        }
    }
}