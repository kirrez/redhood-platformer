using Platformer;
using System.Collections.Generic;

public class PlayerState : IPlayerState
{
    public int ID { get; set; } //slot 0,1 or 2
    public string Name { get; set; } // name of a game

    // Date
    public int DateYear { get; set; }

    public int DateMonth { get; set; }

    public int DateDay { get; set; }

    // Time
    public int TimeHours { get; set; }

    public int TimeMinutes { get; set; }

    // Elapsed Time
    public int ElapsedHours { get; set; }
    public int ElapsedMinutes { get; set; }

    public int DifficultyMode { get; set; }

    //public float ElapsedTime { get; set; } // time spent in game

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
