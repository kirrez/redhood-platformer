using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Navigation : INavigation
    {
        //public delegate void CheckpointChanger(int id);
        public event Action ChangingCheckpoint = () => { };

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

        public void SetLocation(int locationIndex, int spawnPointIndex, int confinerIndex, int musicIndex = 0)
        {
            CurrentStage.SetLocation(locationIndex, spawnPointIndex, confinerIndex);
        }

        public void ChangeCheckpoint()
        {
            ChangingCheckpoint?.Invoke();
        }
    }
}