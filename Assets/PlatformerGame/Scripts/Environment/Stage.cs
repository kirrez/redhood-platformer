using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class Stage : MonoBehaviour
    {
        public List<Transform> SpawnPoints;

        public List<LocationConfig> LocationTransitions;

        private IPlayer Player;
        private CinemachineVirtualCamera VirtualPlayerCamera;
        private CinemachineConfiner Confiner;

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();

            VirtualPlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            VirtualPlayerCamera.Follow = Player.Transform;

            Confiner = VirtualPlayerCamera.GetComponent<CinemachineConfiner>();
        }

        public void SetLocation(int LocationIndex, int SpawnPointIndex)
        {
            var originLocation = LocationTransitions[LocationIndex].OriginLocation;
            var targetLocation = LocationTransitions[LocationIndex].TargetLocation;
            var confiner = LocationTransitions[LocationIndex].Confiner;

            for (int i = 0; i < originLocation.Count; i++)
            {
                originLocation[i].SetActive(false);
            }

            for (int i = 0; i < targetLocation.Count; i++)
            {
                targetLocation[i].SetActive(true);
            }

            Player.Position = SpawnPoints[SpawnPointIndex].position;

            Confiner.m_BoundingShape2D = confiner;
        }

        //public void LoadStage(EStages newStage, int LocationIndex, int SpawnPointIndex)
        //{
        //    Stage stageInstance = ResourceManager.CreatePrefab<Stage, EStages>(newStage);
        //    // is it safe?
        //    var game = CompositionRoot.GetGame();
        //    game.SetStage(stageInstance);

        //    stageInstance.SwitchLocation(LocationIndex, SpawnPointIndex);
        //}
    }
}