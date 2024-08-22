namespace Platformer
{
    public interface IPlayerState
    {
        string Name { get; set; }

        void UpdateTimeAndDate();

        int GetQuest(EQuest quest);
        void SetQuest(EQuest quest, int state);
        void AddValue(EQuest key, int value);
    }
}
