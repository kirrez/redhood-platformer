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

        private IResourceManager ResourceManager;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();
            var dynamicsContainer = CompositionRoot.GetDynamicsContainer();
            var progressManager = CompositionRoot.GetProgressManager();
            progressManager.ResetProgress();

            UIRoot = CompositionRoot.GetUIRoot();

            CompositionRoot.LoadStage(EStages.TheVillage);
            CompositionRoot.SetLocation(0, 0);

            Player = CompositionRoot.GetPlayer();
        }
    }
}