using System.Collections.Generic;

namespace Platformer
{
    public class ProgressManager : IProgressManager
    {
        private Dictionary<EQuest, int> Quests = new Dictionary<EQuest, int>();

        public void ResetProgress()
        {
            Quests.Add(EQuest.Stage, 0);
            Quests.Add(EQuest.SpawnPoint, 0);

            Quests.Add(EQuest.SubWeaponEnabled, 0);

            //Keys
            Quests.Add(EQuest.RedKey, 0);
            Quests.Add(EQuest.GreyKey, 0);
            Quests.Add(EQuest.GreenKey, 0);

            //Mother's quest
            Quests.Add(EQuest.MotherPie, 0);

            //Mother's Pie quest
            Quests.Add(EQuest.MushroomsCurrent, 0);
            Quests.Add(EQuest.MushroomsRequired, 3);
            Quests.Add(EQuest.BerriesCurrent, 0);
            Quests.Add(EQuest.BerriesRequired, 4);

            Quests.Add(EQuest.Mushroom1, 0);
            Quests.Add(EQuest.Mushroom2, 0);
            Quests.Add(EQuest.Mushroom3, 0);
            Quests.Add(EQuest.Blackberry1, 0);
            Quests.Add(EQuest.Blackberry2, 0);
            Quests.Add(EQuest.Blackberry3, 0);
            Quests.Add(EQuest.Blackberry4, 0);
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