using UnityEngine;
using Platformer.ScriptedAnimator;

namespace Platformer
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private Collider2D Block;

        [SerializeField]
        [Range(0, 2)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest KeyItem;

        //advanced player's presence detection
        private Collider2D Collider;
        private IPlayer Player;
        private bool Inside;

        private IDoorAnimator DoorAnimator;
        private IProgressManager ProgressManager;

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
            DoorAnimator = GetComponent<IDoorAnimator>();
            ProgressManager = CompositionRoot.GetProgressManager();

            TargetValue = (int)EQuest.KeyRed + ItemIndex;
            KeyItem = (EQuest)TargetValue;
        }


        private void Update()
        {
            if (Player == null) return;

            if (Collider.bounds.Contains(Player.Position) && !Inside)
            {
                if (ProgressManager.GetQuest(KeyItem) == 1)
                {
                    Block.enabled = false;
                    DoorAnimator.AnimateOpen();
                    Inside = true;
                }
            }

            if (!Collider.bounds.Contains(Player.Position) && Inside)
                {
                if (ProgressManager.GetQuest(KeyItem) == 1)
                {
                    Block.enabled = true;
                    DoorAnimator.AnimateClose();
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