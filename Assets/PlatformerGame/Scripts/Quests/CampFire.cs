using UnityEngine;

namespace Platformer
{
    public class CampFire : BaseQuest
    {
        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int ConfinerIndex;

        [SerializeField]
        private Sprite FireOff;

        [SerializeField]
        private Sprite FireOn;

        [SerializeField]
        private GameObject Fire;

        [SerializeField]
        private SpriteRenderer Renderer;

        private IStorage Storage;
        private INavigation Navigation;

        private float Timer;
        private float Delay = 2f;

        private delegate void State();
        private State CurrentState = () => { };

        protected override void Awake()
        {
            base.Awake();
            Storage = CompositionRoot.GetStorage();
            Navigation = CompositionRoot.GetNavigation();
        }

        protected override void Update()
        {
            // CurrentState works incorrect here
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void OnEnable()
        {
            Navigation.ChangingCheckpoint += OnCheckpointChanged;

            if (ProgressManager.GetQuest(EQuest.SpawnPoint) == SpawnPointIndex)
            {
                SwitchFire(true);
            }
            else
            {
                SwitchFire(false);
            }

            Inside = false;
            Timer = Delay;

            CurrentState = StateRest;
        }

        private void OnDisable()
        {
            Navigation.ChangingCheckpoint -= OnCheckpointChanged;
        }

        private void OnCheckpointChanged()
        {
            if (SpawnPointIndex != ProgressManager.GetQuest(EQuest.SpawnPoint))
            {
                SwitchFire(false);
            }
        }

        private void SwitchFire(bool active)
        {
            if (active)
            {
                Renderer.sprite = FireOn;
                Fire.SetActive(true);
            }

            if (!active)
            {
                Renderer.sprite = FireOff;
                Fire.SetActive(false);
            }
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

        protected override void RequirementsCheck()
        {
            var quest = ProgressManager.GetQuest(EQuest.SpawnPoint);

            if (quest != SpawnPointIndex)
            {
                if (Trigger.bounds.Contains(Player.Position) == true && !Inside)
                {
                    Inside = true;
                    Player.Interaction += OnKindleFire;
                    ShowMessage(Localization.Label(ELabels.KindleAFire));
                }

                if (Trigger.bounds.Contains(Player.Position) == false && Inside)
                {
                    Inside = false;
                    Player.Interaction -= OnKindleFire;
                    HideMessage();
                }
            }

            if (quest == SpawnPointIndex)
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
        }

        private void OnSaveGame()
        {
            Player.HoldByInteraction();

            ProgressManager.AddPlayedTime();
            Storage.Save(ProgressManager.PlayerState);

            // add visual effect
            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);

            Inside = false;
            HideMessage();
            Player.ReleasedByInteraction();
            Player.Interaction -= OnSaveGame;

            Timer = Delay * 2;
            CurrentState = StateRest;
        }

        private void OnKindleFire()
        {
            Player.HoldByInteraction();
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);

            ProgressManager.AddPlayedTime();
            Storage.Save(ProgressManager.PlayerState);

            SwitchFire(true);
            Navigation.ChangeCheckpoint();

            //add visual effect
            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);
            Player.UpdateMaxLives(); //refills health and updates HUD..

            Inside = false;
            HideMessage();
            Player.ReleasedByInteraction();
            Player.Interaction -= OnKindleFire;

            Timer = Delay * 2;
            CurrentState = StateRest;
        }
    }
}