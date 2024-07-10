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

        protected override void Awake()
        {
            base.Awake();
            Storage = CompositionRoot.GetStorage();
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
            Storage.Save(ProgressManager.PlayerState);

            // add visual effect
            AudioManager.PlayRedhoodSound(EPlayerSounds.LightCampFire);

            Player.UpdateMaxLives();
            Player.ReleasedByInteraction();
            Player.Interaction -= OnSaveGame;
        }
    }
}