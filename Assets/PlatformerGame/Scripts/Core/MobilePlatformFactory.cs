namespace Platformer
{
    public class MobilePlatformFactory : IPlatformFactory
    {
        private IResourceManager ResourceManager;

        public MobilePlatformFactory(IResourceManager resourceManager)
        {
            ResourceManager = resourceManager;
        }

        public IUserInput CreateUserInput()
        {
            var userInput = ResourceManager.CreatePrefab<IUserInput, EComponents>(EComponents.TouchInput);
            return userInput;
        }
    }
}