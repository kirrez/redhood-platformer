using System.Collections.Generic;
using System;

namespace Platformer
{
    public class ProgressManager : IProgressManager
    {
        private Dictionary<EQuest, int> Quests = new Dictionary<EQuest, int>();

        public void ResetProgress()
        {
            var type = typeof(EQuest);
            foreach (EQuest item in Enum.GetValues(type))
            {
                Quests.Add(item, 0);
            }

            //Mother's Pie quest
            SetQuest(EQuest.MushroomsRequired, 3);
            SetQuest(EQuest.BerriesRequired, 4);

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