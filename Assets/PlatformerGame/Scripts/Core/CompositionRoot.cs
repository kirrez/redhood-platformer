using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


namespace Platformer
{
    public class CompositionRoot : MonoBehaviour
    {
        private static Game Game;
        private static IUIRoot UIRoot;
        private static GameObject MainCMCamera;
        private static IDynamicsContainer DynamicsContainer;
        private static CinemachineVirtualCamera VirtualPlayerCamera;
        private static GameObject EventSystemContainer;

        private static IPlayer Player;

        private static IResourceManager ResourceManager;
        private static IProgressManager ProgressManager;
        private static ILocalization Localization;

        private static Stage CurrentStage;

        public static void LoadStage(EStages stage)
        {
            if (CurrentStage != null)
            {
                Destroy(CurrentStage.gameObject);
                CurrentStage = null;
            }

            Stage instance = ResourceManager.CreatePrefab<Stage, EStages>(stage);
            CurrentStage = instance;
        }

        public static void SetLocation(int locationIndex = 0, int spawnPointIndex = 0)
        {
            CurrentStage.SetLocation(locationIndex, spawnPointIndex);
        }

        public static IDynamicsContainer GetDynamicsContainer()
        {
            if (DynamicsContainer == null)
            {
                var resourceManager = GetResourceManager();
                DynamicsContainer = resourceManager.CreatePrefab<IDynamicsContainer, EComponents>(EComponents.DynamicsContainer);
            }

            return DynamicsContainer;
        }

        public static Game GetGame()
        {
            if (Game == null)
            {
                var resourceManager = GetResourceManager();
                Game = resourceManager.CreatePrefab<Game, EComponents>(EComponents.Game);
                Debug.Log("Game CREATED");
            }
            return Game;
        }

        public static IPlayer GetPlayer()
        {
            if (Player == null)
            {
                var resourceManager = GetResourceManager();
                //under development
                //Player = resourceManager.CreatePrefab<IPlayer, EComponents>(EComponents.Player);
                Player = resourceManager.CreatePrefab<IPlayer, EComponents>(EComponents.Player2);
            }
            return Player;
        }
        
        public static IUIRoot GetUIRoot()
        {
            if (UIRoot == null)
            {
                var resourceManager = GetResourceManager();

                UIRoot = resourceManager.CreatePrefab<IUIRoot, EComponents>(EComponents.UIRoot);
            }
            return UIRoot;
        }

        public static GameObject GetEventSystem()
        {
            if (EventSystemContainer == null)
            {
                var resourceManager = GetResourceManager();

                EventSystemContainer = resourceManager.CreatePrefab<GameObject, EComponents>(EComponents.EventSystemContainer);
            }
            return EventSystemContainer;
        }

        public static GameObject GetMainCMCamera()
        {
            if (MainCMCamera == null)
            {
                var resourceManager = GetResourceManager();

                MainCMCamera = resourceManager.CreatePrefab<GameObject, EComponents>(EComponents.MainCMCamera);
            }

            return MainCMCamera;
        }

        public static CinemachineVirtualCamera GetVirtualPlayerCamera()
        {
            if (VirtualPlayerCamera == null)
            {
                var resourceManager = GetResourceManager();

                VirtualPlayerCamera = resourceManager.CreatePrefab<CinemachineVirtualCamera, EComponents>(EComponents.VirtualPlayerCamera);
            }

            return VirtualPlayerCamera;
        }

        public static IResourceManager GetResourceManager()
        {
            if (ResourceManager == null)
            {
                ResourceManager = new ResourceManager();
            }

            return ResourceManager;
        }

        public static IProgressManager GetProgressManager()
        {
            if (ProgressManager == null)
            {
                ProgressManager = new ProgressManager();
            }

            return ProgressManager;
        }

        public static ILocalization GetLocalization()
        {
            if (Localization == null)
            {
                Localization = new Localization();
            }

            return Localization;
        }

        private void OnDestroy()
        {
            Game = null;
            Player = null;
            UIRoot = null;
            MainCMCamera = null;
            CurrentStage = null;
            ResourceManager = null;
            VirtualPlayerCamera = null;
            EventSystemContainer = null;
            DynamicsContainer = null;
        }
    }
}