namespace Platformer
{
    public interface IProgressManager
    {
        IPlayerState PlayerState { get; }

        IPlayerState CreateState(int ID, string name);
        void SetState(IPlayerState playerState);

        void RefillRenewables();
        int GetQuest(EQuest key);
        void SetQuest(EQuest key, int value);
        void AddValue(EQuest key, int value);
    }
}