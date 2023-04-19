using UnityEngine;

namespace Platformer
{
    public class DoorGrey : MonoBehaviour
    {
        [SerializeField]
        private Collider2D Block;

        //advanced player's presence detection
        private Collider2D Collider;
        private IPlayer Player;
        private bool Inside;

        private DoorGreyAnimator DoorGreyAnimator;
        private IProgressManager ProgressManager;

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
            DoorGreyAnimator = GetComponent<DoorGreyAnimator>();
            ProgressManager = CompositionRoot.GetProgressManager();
        }


        private void Update()
        {
            if (Player != null)
            {
                if (Collider.bounds.Contains(Player.Position) && !Inside)
                {
                    if (ProgressManager.GetQuest(EQuest.GreyKey) == 1)
                    {
                        Block.enabled = false;
                        DoorGreyAnimator.AnimateOpen();

                        Inside = true;
                    }
                }

                if (!Collider.bounds.Contains(Player.Position) && Inside)
                {
                    if (ProgressManager.GetQuest(EQuest.GreyKey) == 1)
                    {
                        Block.enabled = true;
                        DoorGreyAnimator.AnimateClose();

                        Inside = false;
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = null;
            }
        }
    }
}