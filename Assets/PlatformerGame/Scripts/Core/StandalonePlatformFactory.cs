namespace Platformer
{
    public class StandalonePlatformFactory : IPlatformFactory
    {
        private IResourceManager ResourceManager;

        public StandalonePlatformFactory(IResourceManager resourceManager)
        {
            ResourceManager = resourceManager;
        }

        public IUserInput CreateUserInput()
        {
            var userInput = ResourceManager.CreatePrefab<IUserInput, EComponents>(EComponents.KeyboardInput);
            return userInput;
        }
    }
}