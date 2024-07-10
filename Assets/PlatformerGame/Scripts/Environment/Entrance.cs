using UnityEngine;

namespace Platformer
{
    public class Entrance : MonoBehaviour
    {
        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private ILocalization Localization;
        private IAudioManager AudioManager;
        private IPlayer Player;
        private MessageCanvas Message = null;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int ConfinerIndex;

        [SerializeField]
        private ELabels Name;

        [SerializeField]
        private Transform MessageTransform;

        [SerializeField]
        private bool NoDynamicsReload;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            Localization = CompositionRoot.GetLocalization();
            AudioManager = CompositionRoot.GetAudioManager();
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                ShowMessage(Localization.Label(Name));
                Player.Interaction += OnLocationEnter;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                HideMessage();
                Player.Interaction -= OnLocationEnter;
            }
        }

        private void OnDisable()
        {
            HideMessage();
            if (Player != null)
            {
                Player.Interaction -= OnLocationEnter;
            }
        }

        private void OnLocationEnter()
        {
            var navigation = CompositionRoot.GetNavigation();
            Player.HoldByInteraction();

            if (NoDynamicsReload == false)
            {
                DynamicsContainer.DeactivateMain();
                DynamicsContainer.DeactivateEnemies();
                DynamicsContainer.DeactivateTemporary(); // temporary ))

            }

            navigation.SetLocation(LocationIndex, SpawnPointIndex, ConfinerIndex);
            AudioManager.PlaySound(ESounds.StairSteps);

            var game = CompositionRoot.GetGame();
            Game.FadeScreen.DelayBefore(Color.black, 0.5f);
            Game.FadeScreen.FadeOut(Color.black, 1f);
            Player.ReleasedByInteraction();
        }

    }
}