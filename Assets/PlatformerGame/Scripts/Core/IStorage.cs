namespace Platformer
{
    public interface IStorage
    {
        bool IsExists(int id);

        IPlayerState Load(int id);
        void Save(int id, IPlayerState playerState);
        void Delete(int id);
    }
}