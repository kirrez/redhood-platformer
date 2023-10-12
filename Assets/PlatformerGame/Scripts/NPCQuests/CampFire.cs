using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class CampFire : MonoBehaviour
    {
        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int ConfinerIndex;

        [SerializeField]
        private Sprite FireOff;

        [SerializeField]
        private Sprite FireOn;

        [SerializeField]
        private GameObject Fire;

        [SerializeField]
        Text HelpText;

        private IProgressManager ProgressManager;
        private ILocalization Localization;
        private SpriteRenderer Renderer;
        private IPlayer Player;

        private Collider2D AreaTrigger;
        private bool Inside = false;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
            Renderer = GetComponent<SpriteRenderer>();
            Player = CompositionRoot.GetPlayer();

            HelpText.text = Localization.Text(ETexts.KindleFire);
            AreaTrigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            HelpText.gameObject.SetActive(false);

            if (ProgressManager.GetQuest(EQuest.SpawnPoint) == SpawnPointIndex)
            {
                SwitchFire(true);
            }
            else
            {
                SwitchFire(false);
            }
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.SpawnPoint) == SpawnPointIndex) return;

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

            SwitchFire(false); //every frame..
        }

        private void OnInteraction()
        {
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);
            SwitchFire(true);

            HelpText.gameObject.SetActive(false);
            Player.Interaction -= OnInteraction;
        }

        private void SwitchFire(bool active)
        {
            if (active)
            {
                Renderer.sprite = FireOn;
                Fire.SetActive(true);
            }

            if (!active)
            {
                Renderer.sprite = FireOff;
                Fire.SetActive(false);
            }
        }
    }
}