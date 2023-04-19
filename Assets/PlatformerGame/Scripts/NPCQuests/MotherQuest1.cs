using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class MotherQuest1 : MonoBehaviour
    {
        [SerializeField]
        Transform KeyPosition;

        [SerializeField]
        ETexts Label;

        [SerializeField]
        Text HelpText;

        [SerializeField]
        GameObject NextQuest;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private ILocalization Localization;

        private IPlayer Player;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            Localization = CompositionRoot.GetLocalization();
        }

        private void Start()
        {
            HelpText.text = Localization.Text(Label);
        }

        private void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                Player.Interaction += OnQuestCompleted;
                HelpText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (ProgressManager.GetQuest(EQuest.MotherPie) != 0) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                if (Player != null)
                {
                    Player.Interaction -= OnQuestCompleted;
                }
                HelpText.gameObject.SetActive(false);
            }
        }

        private void OnQuestCompleted()
        {
            ProgressManager.SetQuest(EQuest.MotherPie, 1);
            HelpText.gameObject.SetActive(false);
            Player.Interaction -= OnQuestCompleted;

            var instance = ResourceManager.CreatePrefab<KeyRed, Objects>(Objects.KeyRed);
            instance.transform.position = KeyPosition.position;
        }
    }
}