using UnityEngine;

namespace Platformer
{
    public class Blackberry : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 3)]
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

            TargetValue = (int)EQuest.Blackberry0 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    //increment blackberry current
                    ProgressManager.AddValue(EQuest.BlackberriesCollected, 1);
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