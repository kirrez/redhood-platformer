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
        private Transform MessageTransform;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private ILocalization Localization;
        private SpriteRenderer Renderer;
        private IPlayer Player;

        private MessageCanvas Message = null;
        private Collider2D AreaTrigger;
        private bool Inside = false;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Localization = CompositionRoot.GetLocalization();
            Renderer = GetComponent<SpriteRenderer>();
            Player = CompositionRoot.GetPlayer();

            //HelpText.text = Localization.Text(ETexts.KindleFire);

            AreaTrigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
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
                ShowMessage(Localization.Text(ETexts.KindleFire));
            }

            if (!AreaTrigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
                HideMessage();
            }

            SwitchFire(false); //every frame..
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
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);
            SwitchFire(true);
            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);

            HideMessage();
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