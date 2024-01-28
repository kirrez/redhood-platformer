namespace Platformer
{
    public interface IPlayerState
    {
        int ID { get; set; }
        string Name { get; set; }
        float ElapsedTime { get; set; }

        int GetQuest(EQuest quest);
        void SetQuest(EQuest quest, int state);
        void AddValue(EQuest key, int value);
    }
}
