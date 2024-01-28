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
        private IStorage Storage;

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
            Storage = CompositionRoot.GetStorage();

            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();

            ////Check player state before Slot choosing
            //var playerState1 = Storage.LoadPlayerState(1);
            //var playerState2 = Storage.LoadPlayerState(2);
            //var playerState3 = Storage.LoadPlayerState(3);

            //// Show UI
            ////Create Slot1
            //playerState1 = ProgressManager.CreateState(1, "name");
            ////Create Slot2
            //playerState2 = ProgressManager.CreateState(2, "name");
            ////Create Slot3
            //playerState3 = ProgressManager.CreateState(3, "name");
            ////

            ////Start Game
            //ProgressManager.SetState(playerState2);

            ////Save Game
            //var playerStateToSave = ProgressManager.PlayerState;
            //Storage.Save(playerStateToSave);

            var playerState = LoadTestConfig();
            ProgressManager.SetState(playerState);

            //ProgressManager.LoadTestConfig1();
            //ProgressManager.LoadTestConfig();
            //ProgressManager.LoadNewGame();

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

        private void SaveGame()
        {

        }





        public IPlayerState LoadTestConfig()
        {
            var playerState = ProgressManager.CreateState(1, "test");

            //Testing game mode
            playerState.SetQuest(EQuest.GameMode, 1);// normal, few tries
            playerState.SetQuest(EQuest.TriesLeft, 2);

            //Visited WF, took the pie
            playerState.SetQuest(EQuest.MotherPie, 3);
            playerState.SetQuest(EQuest.MarketElevator, 1);
            //Got red key
            playerState.SetQuest(EQuest.KeyRed, 1);
            //Upper village testing
            playerState.SetQuest(EQuest.KeyGrey, 1);
            //SetQuest(EQuest.KeyGreen, 1);
            //Repaired bridge (BearQuest, Suspension bridge)
            playerState.SetQuest(EQuest.SuspensionBridge, 3);

            //new Player's location
            //CaveLabyrinth Cave8 : Loc. 1, SP : 16, Conf : 8
            //CaveLabyrinth Cave11Boss : L 1, Sp 23, Conf 11

            //SetQuest(EQuest.Stage, (int)EStages.CaveLabyrinth);
            //SetQuest(EQuest.Location, 1);
            //SetQuest(EQuest.SpawnPoint, 23);
            //SetQuest(EQuest.Confiner, 11);

            //Cave Labyrinth
            //SetQuest(EQuest.Stage, (int)EStages.CaveLabyrinth);
            //SetQuest(EQuest.Location, 0);
            //SetQuest(EQuest.SpawnPoint, 0);
            //SetQuest(EQuest.Confiner, 1);

            //Mountains - second camp
            playerState.SetQuest(EQuest.Stage, (int)EStages.Mountains);
            playerState.SetQuest(EQuest.Location, 0);
            playerState.SetQuest(EQuest.SpawnPoint, 2);
            playerState.SetQuest(EQuest.Confiner, 0);

            playerState.SetQuest(EQuest.KnifeLevel, 1);
            playerState.SetQuest(EQuest.AxeLevel, 1);
            playerState.SetQuest(EQuest.HolyWaterLevel, 0);

            playerState.SetQuest(EQuest.MaxLives, 5);
            playerState.SetQuest(EQuest.FoodCollected, 0);
            playerState.SetQuest(EQuest.OreCollected, 0);

            //Finished Mother's Pie quest will be with mushrooms 3 and berries 4
            playerState.SetQuest(EQuest.MushroomsCollected, 3);
            playerState.SetQuest(EQuest.BlackberriesCollected, 4);

            return playerState;
        }
    }
}