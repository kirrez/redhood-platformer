using UnityEngine;

namespace Platformer
{
    public class Ore : MonoBehaviour
    {
        // base - Ore00

        [SerializeField]
        [Range(0, 20)]
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

            TargetValue = (int)EQuest.Ore00 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    //increment food collected
                    ProgressManager.AddValue(EQuest.OreCollected, 1);

                    Game.HUD.UpdateResourceAmount();
                }
                AudioManager.PlaySound(ESounds.Collect6CandyOre);

                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}