using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Navigation : INavigation
    {
        private Stage CurrentStage;

        private IResourceManager ResourceManager;
        public Navigation()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
        }

        public void LoadStage(EStages stage)
        {
            if (CurrentStage != null)
            {
                GameObject.Destroy(CurrentStage.gameObject);
                CurrentStage = null;
            }

            Stage instance = ResourceManager.CreatePrefab<Stage, EStages>(stage);
            CurrentStage = instance;
        }

        public void SetLocation(int locationIndex = 0, int spawnPointIndex = 0, int confinerIndex = 0)
        {
            CurrentStage.SetLocation(locationIndex, spawnPointIndex, confinerIndex);
        }
    }
}