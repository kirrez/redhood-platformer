using UnityEngine;

namespace Platformer
{
    public class Mushroom : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 2)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest Item;

        private IProgressManager ProgressManager;
        private Rigidbody2D Rigidbody;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            PhysicsOn(false);

            TargetValue = (int)EQuest.Mushroom0 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    //increment MushroomsCollected
                    ProgressManager.AddValue(EQuest.MushroomsCollected, 1);
                }
                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}