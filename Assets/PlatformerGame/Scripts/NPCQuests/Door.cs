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

        private Collider2D Collider;
        private IPlayer Player;

        private float Timer;

        private IDoorAnimator DoorAnimator;
        private IAudioManager AudioManager;
        private IProgressManager ProgressManager;

        private delegate void State();
        private State CurrentState = () => { };

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            Collider = GetComponent<Collider2D>();
            DoorAnimator = GetComponent<IDoorAnimator>();
            AudioManager = CompositionRoot.GetAudioManager();
            ProgressManager = CompositionRoot.GetProgressManager();

            TargetValue = (int)EQuest.KeyRed + ItemIndex;
            KeyItem = (EQuest)TargetValue;
        }

        private void OnEnable()
        {
            CurrentState = WaitForOpening;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void WaitForOpening()
        {
            if (Collider.bounds.Contains(Player.Position))
            {
                if (ProgressManager.GetQuest(KeyItem) == 1)
                {
                    Block.enabled = false;
                    DoorAnimator.AnimateOpen();
                    AudioManager.PlaySound(ESounds.DoorHeavy);
                    Timer = 1f;
                    CurrentState = OpeningInProcess;
                }
            }
        }

        private void OpeningInProcess()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer <= 0)
            {
                CurrentState = WaitForClosing;
            }
        }

        private void WaitForClosing()
        {
            if (!Collider.bounds.Contains(Player.Position))
            {
                if (ProgressManager.GetQuest(KeyItem) == 1)
                {
                    Block.enabled = true;
                    DoorAnimator.AnimateClose();
                    AudioManager.PlaySound(ESounds.DoorHeavy);
                    Timer = 1f;
                    CurrentState = ClosingInProcess;
                }
            }
        }

        private void ClosingInProcess()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer <= 0)
            {
                CurrentState = WaitForOpening;
            }
        }
    }
}