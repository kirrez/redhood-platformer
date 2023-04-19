using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class Game : MonoBehaviour
    {
        private IPlayer Player;
        private IUIRoot UIRoot;
        private CinemachineVirtualCamera VirtualPlayerCamera;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;

        private Stage CurrentStage;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            
            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();

            ProgressManager = CompositionRoot.GetProgressManager();
            ProgressManager.ResetProgress();

            UIRoot = CompositionRoot.GetUIRoot();

            //temporary solution
            //CurrentStage = ResourceManager.CreatePrefab<Stage, EStages>(EStages.TheVillage);

            CurrentStage = ResourceManager.CreatePrefab<Stage, EStages>(EStages.WestForest);

            Player = CompositionRoot.GetPlayer();
            Player.Position = CurrentStage.SpawnPoints[0].position;
            
            VirtualPlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            VirtualPlayerCamera.Follow = Player.Transform;
        }

    }
}