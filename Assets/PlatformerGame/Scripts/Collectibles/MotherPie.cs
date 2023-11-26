using UnityEngine;

namespace Platformer
{
    public class MotherPie : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private Rigidbody2D Rigidbody;

        private void OnEnable()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            PhysicsOn(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(EQuest.MotherPie) == 4)
                {
                    ProgressManager.SetQuest(EQuest.MotherPie, 5); // Finishing MotherPie quest
                    ProgressManager.SetQuest(EQuest.MarketElevator, 1); // Unlocking elevator
                }
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