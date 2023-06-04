using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using Platformer.UI;
using UnityEngine;

namespace Platformer
{
    public class Game : MonoBehaviour
    {
        //private IPlayer Player;
        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;


        public DialogueModel Dialogue;

        private void Awake()
        {
            Debug.Log("Game Constructor");
            ResourceManager = CompositionRoot.GetResourceManager();
            var eventSystem = CompositionRoot.GetEventSystem();
            var mainCMCamera = CompositionRoot.GetMainCMCamera();
            var dynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ProgressManager = CompositionRoot.GetProgressManager();
            ProgressManager.ResetProgress();

            //initial location data saving
            ProgressManager.SetQuest(EQuest.Stage, (int)EStages.TheVillage);
            ProgressManager.SetQuest(EQuest.SpawnPoint, 0);

            Dialogue = new DialogueModel();
            Dialogue.Hide();

            CompositionRoot.LoadStage(EStages.TheVillage);
            CompositionRoot.SetLocation(0, 0);

            //Player = CompositionRoot.GetPlayer();
        }
    }
}