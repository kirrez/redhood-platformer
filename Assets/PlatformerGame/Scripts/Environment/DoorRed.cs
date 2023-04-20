using UnityEngine;

namespace Platformer
{
    public class DoorRed : MonoBehaviour
    {
        [SerializeField]
        private Collider2D Block;

        //advanced player's presence detection
        private Collider2D Collider;
        private IPlayer Player;
        private bool Inside;

        private DoorRedAnimator DoorRedAnimator;
        private IProgressManager ProgressManager;

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
            DoorRedAnimator = GetComponent<DoorRedAnimator>();
            ProgressManager = CompositionRoot.GetProgressManager();
        }


        private void Update()
        {
            if (Player == null) return;

            if (Collider.bounds.Contains(Player.Position) && !Inside)
            {
                if (ProgressManager.GetQuest(EQuest.RedKey) == 1)
                {
                    Block.enabled = false;
                    DoorRedAnimator.AnimateOpen();

                    Inside = true;
                }
            }

            if (!Collider.bounds.Contains(Player.Position) && Inside)
                {
                if (ProgressManager.GetQuest(EQuest.RedKey) == 1)
                {
                    Block.enabled = true;
                    DoorRedAnimator.AnimateClose();

                    Inside = false;
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