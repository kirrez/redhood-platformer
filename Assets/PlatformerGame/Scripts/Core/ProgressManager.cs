namespace Platformer
{
    public class ProgressManager : IProgressManager
    {
        public IPlayerState PlayerState { get; private set; }

        public IPlayerState CreateState(int id, string name)
        {
            var playerState = new PlayerState();
            playerState.ID = id;
            playerState.Name = name;

            playerState.SetQuest(EQuest.GameMode, 0);// easy, infinite tries

            playerState.SetQuest(EQuest.MaxLives, 3);
            playerState.SetQuest(EQuest.MaxLivesCap, 8);
            playerState.SetQuest(EQuest.LifeUpgradeCost, 5);

            playerState.SetQuest(EQuest.KnifeLevel, 1);
            playerState.SetQuest(EQuest.AxeLevel, 0);
            playerState.SetQuest(EQuest.HolyWaterLevel, 0);

            //Start Player's location
            playerState.SetQuest(EQuest.Stage, (int)EStages.TheVillage);
            playerState.SetQuest(EQuest.Location, 0);
            playerState.SetQuest(EQuest.SpawnPoint, 0);
            playerState.SetQuest(EQuest.Confiner, 0);

            playerState.SetQuest(EQuest.KeyRed, -1);
            playerState.SetQuest(EQuest.KeyGrey, -1);
            playerState.SetQuest(EQuest.KeyGreen, -1);

            //Mother's Pie quest
            playerState.SetQuest(EQuest.MushroomsRequired, 3);
            playerState.SetQuest(EQuest.BlackberriesRequired, 4);

            //Boss data
            playerState.SetQuest(EQuest.MegafrogMaxHealth, 90); // 90 (45 * 2, 30 * 3)

            return playerState;
        }

        public void SetState(IPlayerState playerState)
        {
            PlayerState = playerState;
        }

        public int GetQuest(EQuest key)
        {
            return PlayerState.GetQuest(key);
        }

        public void SetQuest(EQuest key, int value)
        {
            PlayerState.SetQuest(key, value);
        }

        public void AddValue(EQuest key, int value)
        {
            PlayerState.AddValue(key, value);
        }

        public void RefillRenewables()
        {
            int targetValue;
            EQuest item;

            int replenisherAmount = 9; // MAGIC NUMBER from HealthReplenisher script

            for (int i = 0; i < replenisherAmount + 1; i++)
            {
                targetValue = (int)EQuest.Replenish00 + i;
                item = (EQuest)targetValue;
                SetQuest(item, 0);
            }
        }
    }
}

//Mother's pie :
//1. Dialogue in MotherQuest1 changes value to 1, spawns key in cellar
//2. FamilyDialogue changes value from 1 to 2
//3. StoryCheckPoint in West Forest changes value from 2 to 3 (for single save of respawn point)
//4. MotherQuest2 gives you a pie, if value is 3, and change it to 4
//5. PieItem changes value from 4 to 5. This opens LifeUpgrade quest and turns on Market elevator (when you enter MarketElevator's trigger)

//Suspension bridge :
//1. SuspensionBridge changes value from 0 to 1 (Breaking bridge) and saves first CheckPoint
//2. BearQuest changes value from 1 to 2, that affects SuspensionBridge immediately
//3. SuspensionBridge changes value from 2 to 3 and repairs the bridge

//Blacksmith's errand :
//1. BlacksmithQuest1 changes value from 0 to 1, now blacksmith is waiting for 3 pieces of iron ore.
//1.1 Soldier near Eastern gates tells you to get your own key from blaksmith, if value is 0. 
//2. BlacksmithQuest2 takes 3 iron ore from inventory and gives you a grey key. Value changes from 1 to 2.