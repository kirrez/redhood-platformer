using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


namespace Platformer
{
    public class CompositionRoot : MonoBehaviour
    {
        private static IGame Game;
        private static IUIRoot UIRoot;
        private static GameObject MainCMCamera;
        private static INavigation Navigation;
        private static IDynamicsContainer DynamicsContainer;
        private static CinemachineVirtualCamera VirtualPlayerCamera;
        private static GameObject EventSystemContainer;

        private static IPlayer Player;

        private static IResourceManager ResourceManager;
        private static IProgressManager ProgressManager;
        private static ILocalization Localization;

        public static IDynamicsContainer GetDynamicsContainer()
        {
            if (DynamicsContainer == null)
            {
                var resourceManager = GetResourceManager();
                DynamicsContainer = resourceManager.CreatePrefab<IDynamicsContainer, EComponents>(EComponents.DynamicsContainer);
            }

            return DynamicsContainer;
        }

        public static IGame GetGame()
        {
            if (Game == null)
            {
                var resourceManager = GetResourceManager();
                Game = resourceManager.CreatePrefab<IGame, EComponents>(EComponents.Game);
            }
            return Game;
        }

        public static IPlayer GetPlayer()
        {
            if (Player == null)
            {
                var resourceManager = GetResourceManager();
                Player = resourceManager.CreatePrefab<IPlayer, EComponents>(EComponents.Player);
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

        public static INavigation GetNavigation()
        {
            if (Navigation == null)
            {
                Navigation = new Navigation();
            }

            return Navigation;
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
            Navigation = null;
            MainCMCamera = null;
            ResourceManager = null;
            VirtualPlayerCamera = null;
            EventSystemContainer = null;
            DynamicsContainer = null;
        }
    }
}