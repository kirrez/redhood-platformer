using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class Game : MonoBehaviour, IGame
    {
        //private IPlayer Player;
        //private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private IDynamicsContainer DynamicsContainer;
        private IAudioManager AudioManager;
        private INavigation Navigation;
        private IPlayer Player;

        public DialogueModel Dialogue => DialogueModel;
        private DialogueModel DialogueModel;

        public FadeScreenModel FadeScreen => FadeScreenModel;
        private FadeScreenModel FadeScreenModel;

        public GameOverModel GameOver => GameOverModel;
        private GameOverModel GameOverModel;

        public HUDModel HUD => HUDModel;
        private HUDModel HUDModel;

        private void Awake()
        {
            //ResourceManager = CompositionRoot.GetResourceManager();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Navigation = CompositionRoot.GetNavigation();

            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();

            //ProgressManager.LoadTestConfig1();
            //ProgressManager.LoadTestConfig();
            ProgressManager.LoadNewGame();

            GameOverModel = new GameOverModel();
            GameOverModel.TryingAgain += TryAgain;
            GameOverModel.Hide();

            DialogueModel = new DialogueModel();
            DialogueModel.Hide();
            
            // as the last screen on MenuCanvas it can be used for fading menues too
            FadeScreenModel = new FadeScreenModel();
            FadeScreenModel.Show();

            HUDModel = new HUDModel();
            HUDModel.Show();//in StartGame
            HUDModel.SetMaxLives(ProgressManager.GetQuest(EQuest.MaxLives));
            HUDModel.UpdateResourceAmount();

            Player = CompositionRoot.GetPlayer();
            Player.Initiate(this);
            Player.Revive();

            //should be in "StartGame" and "ContinueGame"
            LoadPlayerLocation();
        }

        public void GameOverMenu()
        {
            HUDModel.Hide();
            GameOverModel.Show();
        }

        private void LoadPlayerLocation()
        {
            var stage = (EStages)ProgressManager.GetQuest(EQuest.Stage);
            var location = ProgressManager.GetQuest(EQuest.Location);
            var spawnPoint = ProgressManager.GetQuest(EQuest.SpawnPoint);
            var confiner = ProgressManager.GetQuest(EQuest.Confiner);

            Navigation.LoadStage(stage);
            Navigation.SetLocation(location, spawnPoint, confiner);
        }

        private void TryAgain()
        {
            DynamicsContainer.DeactivateMain();
            DynamicsContainer.DeactivateEnemies();
            DynamicsContainer.DeactivateTemporary(); // temp

            ProgressManager.RefillRenewables();
            LoadPlayerLocation();

            GameOverModel.Hide();
            HUDModel.Show();
            Player.Revive();
            AudioManager.ReplayMusic();

            FadeScreenModel.DelayBefore(Color.black, 1f);
            FadeScreenModel.FadeOut(Color.black, 1f);
        }
    }
}