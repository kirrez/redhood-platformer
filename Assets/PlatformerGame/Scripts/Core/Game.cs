using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using Platformer.UI;
using UnityEngine;

namespace Platformer
{
    public class Game : MonoBehaviour, IGame
    {
        //private IPlayer Player;
        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;

        public DialogueModel Dialogue => DialogueModel;
        private DialogueModel DialogueModel;

        private Stage CurrentStage;

        public void LoadStage(EStages stage)
        {
            if (CurrentStage != null)
            {
                Destroy(CurrentStage.gameObject);
                CurrentStage = null;
            }

            Stage instance = ResourceManager.CreatePrefab<Stage, EStages>(stage);
            CurrentStage = instance;
        }

        public void SetLocation(int locationIndex = 0, int spawnPointIndex = 0)
        {
            CurrentStage.SetLocation(locationIndex, spawnPointIndex);
        }

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();
            var dynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ProgressManager = CompositionRoot.GetProgressManager();
            ProgressManager.ResetProgress();

            //initial location data saving
            ProgressManager.SetQuest(EQuest.Stage, (int)EStages.TheVillage);
            ProgressManager.SetQuest(EQuest.SpawnPoint, 0);

            DialogueModel = new DialogueModel();
            DialogueModel.Hide();

            LoadStage(EStages.TheVillage);
            SetLocation(0, 0);

            //Player = CompositionRoot.GetPlayer();
        }
    }
}