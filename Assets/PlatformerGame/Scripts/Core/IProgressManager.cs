namespace Platformer
{
    public interface IProgressManager
    {
        IPlayerState PlayerState { get; }

        //IPlayerState CreateState(int ID, string name);
        IPlayerState CreateState(int ID);

        void SetState(IPlayerState playerState);

        void AddPlayedTime();

        void RefillRenewables();
        int GetQuest(EQuest key);
        void SetQuest(EQuest key, int value);
        void AddValue(EQuest key, int value);
    }
}