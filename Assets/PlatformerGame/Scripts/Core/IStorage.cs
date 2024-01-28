namespace Platformer
{
    public interface IStorage
    {
        bool IsPlayerStateExists(int ID);

        IPlayerState LoadPlayerState(int ID);
        void Save(IPlayerState playerState);
    }
}