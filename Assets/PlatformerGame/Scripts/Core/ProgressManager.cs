using System.Collections.Generic;
using UnityEngine;
using System;

namespace Platformer
{
    public class ProgressManager : IProgressManager
    {
        private Dictionary<EQuest, int> Quests = new Dictionary<EQuest, int>();

        public ProgressManager()
        {
            var type = typeof(EQuest);
            foreach (EQuest item in Enum.GetValues(type))
            {
                Quests.Add(item, 0);
            }
        }

        public void LoadNewGame()
        {
            SetQuest(EQuest.MaxLives, 3);
            SetQuest(EQuest.MaxLivesCap, 8);
            SetQuest(EQuest.LifeUpgradeCost, 10);

            SetQuest(EQuest.KnifeLevel, 1);
            SetQuest(EQuest.AxeLevel, 1);
            SetQuest(EQuest.HolyWaterLevel, 0);

            //Start Player's location
            SetQuest(EQuest.Stage, (int)EStages.TheVillage);
            SetQuest(EQuest.Location, 0);
            SetQuest(EQuest.SpawnPoint, 0);

            //Mother's Pie quest
            SetQuest(EQuest.MushroomsRequired, 3);
            SetQuest(EQuest.BlackberriesRequired, 4);
        }

        public void LoadTestConfig()
        {
            LoadNewGame();

            //Visited WF
            SetQuest(EQuest.MotherPie, 3);
            //Got red key
            SetQuest(EQuest.KeyRed, 1);
            //Repaired bridge (BearQuest, Suspension bridge)
            SetQuest(EQuest.SuspensionBridge, 3);

            //new Player's location
            SetQuest(EQuest.Stage, (int)EStages.TheVillage);
            SetQuest(EQuest.Location, 0);
            SetQuest(EQuest.SpawnPoint, 0);

            SetQuest(EQuest.KnifeLevel, 1);
            SetQuest(EQuest.AxeLevel, 1);
            SetQuest(EQuest.HolyWaterLevel, 0);

            SetQuest(EQuest.MaxLives, 4);
            //Alternative costs
            SetQuest(EQuest.LifeUpgradeCost, 2);

            //Finished Mother's Pie quest will be with mushrooms 3 and berries 4
            SetQuest(EQuest.MushroomsCollected, 3);
            SetQuest(EQuest.BlackberriesCollected, 4);
        }

        public int GetQuest(EQuest key)
        {
            return Quests[key];
        }

        public void SetQuest(EQuest key, int value)
        {
            Quests[key] = value;
        }

        public void AddValue(EQuest key, int value)
        {
            Quests[key] += value;
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
//1.1 Soldier near Eastern gates tells you to get your own key from blaksmith, if value if 0. 
//2. BlacksmithQuest2 takes 3 iron ore from inventory and gives you a grey key. Value changes from 1 to 2.