using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class Game : MonoBehaviour
    {
        //temporary
        [SerializeField]
        private EStages Stage;

        private IPlayer Player;
        private IUIRoot UIRoot;

        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;

        private Stage CurrentStage;

        public void SetStage(Stage newStage)
        {
            CurrentStage = newStage;
        }

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

            CurrentStage = ResourceManager.CreatePrefab<Stage, EStages>(Stage);
            CurrentStage.Initiate(this);
            CurrentStage.SwitchLocation(0, 0);

            Player = CompositionRoot.GetPlayer();
            //
        }
    }
}