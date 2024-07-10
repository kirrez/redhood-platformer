using System.Collections.Generic;
using Platformer;
using System;

public class PlayerState : IPlayerState
{
    public int ID { get; set; } //slot 0,1 or 2
    public string Name { get; set; } // name of a game

    private Dictionary<EQuest, int> Quests = new Dictionary<EQuest, int>();

    public void UpdateTimeAndDate()
    {
        DateTime dateTime;
        int data;

        dateTime = DateTime.Now;

        data = dateTime.Year;
        SetQuest(EQuest.DateYear, data);

        data = dateTime.Month;
        SetQuest(EQuest.DateMonth, data);

        data = dateTime.Day;
        SetQuest(EQuest.DateDay, data);

        data = dateTime.Hour;
        SetQuest(EQuest.TimeHours, data);

        data = dateTime.Minute;
        SetQuest(EQuest.TimeMinutes, data);
    }

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
