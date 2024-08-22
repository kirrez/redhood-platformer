namespace Platformer
{
    public interface IProgressManager
    {
        int ID { get; }
        IPlayerState PlayerState { get; }

        //IPlayerState CreateState(int ID, string name);
        IPlayerState CreateState(int id);

        void SetState(IPlayerState playerState);

        void AddPlayedTime();

        void RefillRenewables();
        int GetQuest(EQuest key);
        void SetQuest(EQuest key, int value);
        void AddValue(EQuest key, int value);
    }
}