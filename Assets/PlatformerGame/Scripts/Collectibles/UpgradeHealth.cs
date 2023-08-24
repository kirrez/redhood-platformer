using UnityEngine;

namespace Platformer
{
    public class UpgradeHealth : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private Rigidbody2D Rigidbody;
        private IPlayer Player;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            Player = CompositionRoot.GetPlayer();

            PhysicsOn(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var maxLives = ProgressManager.GetQuest(EQuest.MaxLives);
                var maxLivesCap = ProgressManager.GetQuest(EQuest.MaxLivesCap);
                if (maxLives + 1 <= maxLivesCap)
                {
                    ProgressManager.AddValue(EQuest.MaxLives, 1);
                    ProgressManager.SetQuest(EQuest.UpgradeHealth, 2); //spawner won't spit out items, until variable is not changed back to 1 in quest
                }

                Player.UpdateMaxLives();

                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}