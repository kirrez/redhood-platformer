using UnityEngine;

namespace Platformer
{
    public class Bag600 : MonoBehaviour
    {
        // base - Bag600_00

        private int CurrencyValue = 600;

        [SerializeField]
        [Range(0, 30)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest Item;

        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private Rigidbody2D Rigidbody;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            PhysicsOn(false);

            TargetValue = (int)EQuest.Bag600_00 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(Item, 1);
                ProgressManager.AddValue(EQuest.Money, CurrencyValue);

                var Game = CompositionRoot.GetGame();
                Game.HUD.UpdateResourceAmount();
                AudioManager.PlaySound(ESounds.Collect2MoneyBags);

                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}