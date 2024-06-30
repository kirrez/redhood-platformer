namespace Platformer
{
    public interface IPlayerState
    {
        int ID { get; set; }
        string Name { get; set; }

        int DateYear { get; set; }
        int DateMonth { get; set; }
        int DateDay { get; set; }
        int TimeHours { get; set; }
        int TimeMinutes { get; set; }
        int ElapsedHours { get; set; }
        int ElapsedMinutes { get; set; }

        int DifficultyMode { get; set; }

        int GetQuest(EQuest quest);
        void SetQuest(EQuest quest, int state);
        void AddValue(EQuest key, int value);
    }
}
