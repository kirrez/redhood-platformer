using Platformer;
using System.Collections.Generic;

public class PlayerState : IPlayerState
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float ElapsedTime { get; set; }

    private Dictionary<EQuest, int> Quests = new Dictionary<EQuest, int>();

    public int GetQuest(EQuest key)
    {
        if (Quests.ContainsKey(key) == false)
        {
            Quests.Add(key, 0);
        }

        return Quests[key];
    }

    public void SetQuest(EQuest key, int value)
    {
        if (Quests.ContainsKey(key) == false)
        {
            Quests.Add(key, 0);
        }

        Quests[key] = value;
    }

    public void AddValue(EQuest key, int value)
    {
        if (Quests.ContainsKey(key) == false)
        {
            Quests.Add(key, 0);
        }

        Quests[key] += value;
    }
}
