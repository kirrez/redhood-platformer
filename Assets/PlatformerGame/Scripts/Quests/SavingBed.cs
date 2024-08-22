using UnityEngine;

namespace Platformer
{
    public class SavingBed : BaseQuest
    {
        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int ConfinerIndex;

        private IStorage Storage;

        private float Timer;
        private float Delay = 3f;

        private delegate void State();
        private State CurrentState = () => { };

        protected override void Awake()
        {
            base.Awake();
            Storage = CompositionRoot.GetStorage();
        }

        private void OnEnable()
        {
            Inside = false;
            Timer = Delay;
            CurrentState = StateRest;
        }

        protected override void Update()
        {
            CurrentState();
        }

        private void StateRest()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) return;

            CurrentState = StateCheck;
        }

        private void StateCheck()
        {
            RequirementsCheck();
        }

        private void StateFadeIn()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) return;

            Game.FadeScreen.DelayAfter(1f);
            Timer = 1f;

            CurrentState = StateDelayAfter;
        }

        private void StateDelayAfter()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) return;

            Player.ReleasedByInteraction();
            Game.FadeScreen.FadeOut(Color.black, 1f);
            Timer = Delay + 1f;

            CurrentState = StateRest;
        }

        protected override void RequirementsCheck()
        {
            if (Trigger.bounds.Contains(Player.Position) == true && !Inside)
            {
                Inside = true;
                Player.Interaction += OnSaveGame;
                ShowMessage(Localization.Label(ELabels.SaveGame));
            }

            if (Trigger.bounds.Contains(Player.Position) == false && Inside)
            {
                Inside = false;
                Player.Interaction -= OnSaveGame;
                HideMessage();
            }
        }

        private void OnSaveGame()
        {
            Player.HoldByInteraction();
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);

            ProgressManager.AddPlayedTime();
            Storage.Save(ProgressManager.ID, ProgressManager.PlayerState);

            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);

            Inside = false;
            HideMessage();
            Player.UpdateMaxLives();
            //Player.ReleasedByInteraction();
            Player.Interaction -= OnSaveGame;

            // visual effect
            Timer = 1f;
            Game.FadeScreen.FadeIn(Color.black, 1f);

            CurrentState = StateFadeIn;
        }
    }
}