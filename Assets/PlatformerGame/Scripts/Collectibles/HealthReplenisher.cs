using UnityEngine;

namespace Platformer
{
    public class HealthReplenisher : MonoBehaviour
    {
        // base - Replenish00

        [SerializeField]
        [Range(0, 9)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest Item;

        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private Rigidbody2D Rigidbody;
        private IPlayer Player;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            Player = CompositionRoot.GetPlayer();
            PhysicsOn(false);

            TargetValue = (int)EQuest.Replenish00 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ProgressManager.SetQuest(Item, 1);
                Player.UpdateMaxLives();
                AudioManager.PlaySound(ESounds.Collect3HealthReplenish);

                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}