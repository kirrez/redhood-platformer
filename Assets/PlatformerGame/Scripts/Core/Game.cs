using UnityEngine;

namespace Platformer
{
    public class Game : MonoBehaviour, IGame
    {
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

        public TitleScreenModel TitleScreen => TitleScreenModel;
        private TitleScreenModel TitleScreenModel;

        public PlayScreenModel PlayScreen => PlayScreenModel;
        private PlayScreenModel PlayScreenModel;

        public CreditsScreenModel CreditsScreen => CreditsScreenModel;
        private CreditsScreenModel CreditsScreenModel;

        public SettingsScreenModel SettingsScreen => SettingsScreenModel;
        private SettingsScreenModel SettingsScreenModel;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Navigation = CompositionRoot.GetNavigation();

            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();

            ////Save Game
            //var playerStateToSave = ProgressManager.PlayerState;
            //Storage.Save(playerStateToSave);

            // Only this for "LOAD TEST CONFIG"
            //var playerState = LoadTestConfig();
            //ProgressManager.SetState(playerState);

            // Only for "START NEW GAME"
            //var playerState = ProgressManager.CreateState(1);
            //ProgressManager.SetState(playerState);

            // Title
            TitleScreenModel = new TitleScreenModel();
            TitleScreen.Show();
            TitleScreenModel.ClickingPlay += FromTitleToPlay;
            TitleScreenModel.ClickingSettings += FromTitleToSettings;
            TitleScreenModel.ClickingCredits += FromTitleToCredits;
            TitleScreenModel.ClickingQuit += QuitApplication;

            // Play
            PlayScreenModel = new PlayScreenModel();
            PlayScreenModel.ClickingBackToMenu += FromPlayToTitle;
            PlayScreenModel.ClickingPlayGame += FromPlayToGame;
            PlayScreen.Hide();

            // Credits
            CreditsScreenModel = new CreditsScreenModel();
            CreditsScreenModel.ClickingBackToMenu += FromCreditsToTitle;
            CreditsScreen.Hide();

            // Settings
            SettingsScreenModel = new SettingsScreenModel();
            SettingsScreenModel.ClickingBackToMenu += FromSettingsToTitle;
            SettingsScreen.Hide();
            
            //-----------------
            GameOverModel = new GameOverModel();
            GameOverModel.TryingAgain += TryAgain;
            GameOver.Hide();

            DialogueModel = new DialogueModel();
            Dialogue.Hide();

            HUDModel = new HUDModel();
            HUD.Hide();

            // as the last screen on MenuCanvas it can be used for fading menues too
            FadeScreenModel = new FadeScreenModel();
            FadeScreen.Hide();

            // in the beginning of Game
            //HUD.Show();
            //FadeScreen.Show();
            //HUD.SetMaxLives(ProgressManager.GetQuest(EQuest.MaxLives));
            //HUD.UpdateResourceAmount();

            //Player = CompositionRoot.GetPlayer();
            //Player.Initiate(this);
            //Player.Revive();

            //should be in "StartGame" and "ContinueGame"
            //LoadPlayerLocation();
        }

        public void FromPlayToGame()
        {
            // Here starts actual game process !
            PlayScreen.Hide();

            HUD.Show();
            FadeScreen.Show();
            HUD.SetMaxLives(ProgressManager.GetQuest(EQuest.MaxLives));
            HUD.UpdateResourceAmount();

            Player = CompositionRoot.GetPlayer();
            Player.Initiate(this);
            Player.Revive();

            LoadPlayerLocation();
        }

        public void FromPlayToTitle()
        {
            PlayScreen.Hide();
            TitleScreen.Show();
        }

        public void FromSettingsToTitle()
        {
            SettingsScreen.Hide();
            TitleScreen.Show();
        }

        public void FromCreditsToTitle()
        {
            CreditsScreen.Hide();
            TitleScreen.Show();
        }

        public void FromTitleToPlay()
        {
            TitleScreen.Hide();
            PlayScreen.Show();
        }

        public void FromTitleToSettings()
        {
            TitleScreen.Hide();
            SettingsScreen.Show();
        }

        public void FromTitleToCredits()
        {
            TitleScreen.Hide();
            CreditsScreen.Show();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void GameOverMenu()
        {
            HUD.Hide();
            GameOver.Show();
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

            GameOver.Hide();
            HUD.Show();
            Player.Revive();
            AudioManager.ReplayMusic();

            FadeScreen.DelayBefore(Color.black, 1f);
            FadeScreen.FadeOut(Color.black, 1f);
        }

        private void SaveGame()
        {

        }

        public IPlayerState LoadTestConfig()
        {
            var playerState = ProgressManager.CreateState(1);

            //Testing game mode
            playerState.SetQuest(EQuest.GameMode, 0);// normal, few tries
            playerState.SetQuest(EQuest.TriesLeft, 2);

            //Visited WF, took the pie
            playerState.SetQuest(EQuest.MotherPie, 3);
            playerState.SetQuest(EQuest.MarketElevator, 1);
            //Got red key
            playerState.SetQuest(EQuest.KeyRed, 1);
            //Upper village testing
            //playerState.SetQuest(EQuest.KeyGrey, 1);
            //SetQuest(EQuest.KeyGreen, 1);
            //Repaired bridge
            playerState.SetQuest(EQuest.SuspensionBridge, 3);

            //CaveLabyrinth Cave8 : Loc. 1, SP : 16, Conf : 8
            //CaveLabyrinth Cave11Boss : L 1, Sp 23, Conf 11

            // Cave Labyrinth, start
            //playerState.SetQuest(EQuest.Stage, (int)EStages.CaveLabyrinth);
            //playerState.SetQuest(EQuest.Location, 0);
            //playerState.SetQuest(EQuest.SpawnPoint, 24); // test
            //playerState.SetQuest(EQuest.Confiner, 1);

            // Cave Labyrinth, Campfire boss
            //playerState.SetQuest(EQuest.Stage, (int)EStages.CaveLabyrinth);
            //playerState.SetQuest(EQuest.Location, 2);
            //playerState.SetQuest(EQuest.SpawnPoint, 19);
            //playerState.SetQuest(EQuest.Confiner, 10);

            // Cave Labyrinth, Chapel
            //playerState.SetQuest(EQuest.Stage, (int)EStages.CaveLabyrinth);
            //playerState.SetQuest(EQuest.Location, 0);
            //playerState.SetQuest(EQuest.SpawnPoint, 24);
            //playerState.SetQuest(EQuest.Confiner, 1);

            // Cave Labyrinth
            //playerState.SetQuest(EQuest.Stage, (int)EStages.CaveLabyrinth);
            //playerState.SetQuest(EQuest.Location, 0);
            //playerState.SetQuest(EQuest.SpawnPoint, 24); //46
            //playerState.SetQuest(EQuest.Confiner, 1);    //16

            // Mountains
            //playerState.SetQuest(EQuest.Stage, (int)EStages.Mountains);
            //playerState.SetQuest(EQuest.Location, 0);
            //playerState.SetQuest(EQuest.SpawnPoint, 2); // 2
            //playerState.SetQuest(EQuest.Confiner, 0);

            // StartOrTutorial
            //playerState.SetQuest(EQuest.Stage, (int)EStages.StartOrTutorial);
            //playerState.SetQuest(EQuest.Location, 0);
            //playerState.SetQuest(EQuest.SpawnPoint, 0);
            //playerState.SetQuest(EQuest.Confiner, 0);

            // The Village
            playerState.SetQuest(EQuest.Stage, (int)EStages.WesternForest);
            playerState.SetQuest(EQuest.Location, 0);
            playerState.SetQuest(EQuest.SpawnPoint, 4);
            playerState.SetQuest(EQuest.Confiner, 0);

            playerState.SetQuest(EQuest.KnifeLevel, 1);
            playerState.SetQuest(EQuest.AxeLevel, 0);
            playerState.SetQuest(EQuest.HolyWaterLevel, 0);

            playerState.SetQuest(EQuest.MaxLives, 3);
            playerState.SetQuest(EQuest.FoodCollected, 0);
            playerState.SetQuest(EQuest.OreCollected, 0);

            //Finished Mother's Pie quest will be with mushrooms 3 and berries 4
            playerState.SetQuest(EQuest.MushroomsCollected, 3);
            playerState.SetQuest(EQuest.BlackberriesCollected, 4);

            return playerState;
        }
    }
}