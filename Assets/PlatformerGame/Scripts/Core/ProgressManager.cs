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
            SetQuest(EQuest.AxeLevel, 0);
            SetQuest(EQuest.HolyWaterLevel, 0);

            //Start Player's location
            SetQuest(EQuest.Stage, (int)EStages.TheVillage);
            SetQuest(EQuest.Location, 0);
            SetQuest(EQuest.SpawnPoint, 0);

            //Mother's Pie quest
            SetQuest(EQuest.MushroomsRequired, 3);
            SetQuest(EQuest.BerriesRequired, 4);
        }

        public void LoadTestConfig()
        {
            LoadNewGame();

            //Talked to mom first time, Family monologue
            SetQuest(EQuest.MotherPie, 2);
            //Got red key
            SetQuest(EQuest.RedKey, 1);

            //new Player's location
            SetQuest(EQuest.Stage, (int)EStages.WestForest);
            SetQuest(EQuest.Location, 0);
            SetQuest(EQuest.SpawnPoint, 0);

            SetQuest(EQuest.KnifeLevel, 2);
            SetQuest(EQuest.AxeLevel, 2); // woodBreaker development
            SetQuest(EQuest.HolyWaterLevel, 1);

            SetQuest(EQuest.MaxLives, 4);
            //Alternative costs
            //SetQuest(EQuest.LifeUpgradeCost, 1);

            //Finished Mother's Pie quest will be with mushrooms 3 and berries 4
            //SetQuest(EQuest.MushroomsCurrent, 0);
            //SetQuest(EQuest.BerriesCurrent, 0);
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