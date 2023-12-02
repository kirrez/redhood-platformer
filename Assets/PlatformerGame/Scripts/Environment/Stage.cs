using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class Stage : MonoBehaviour
    {
        public List<Transform> SpawnPoints;

        public List<PolygonCollider2D> Confiners;

        public List<LocationConfig> LocationTransitions;

        public List<EMusic> Music;

        private IPlayer Player;
        private IAudioManager AudioManager;
        
        private CinemachineVirtualCamera VirtualPlayerCamera;
        private CinemachineConfiner Confiner;

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            AudioManager = CompositionRoot.GetAudioManager();

            VirtualPlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            VirtualPlayerCamera.Follow = Player.Transform;

            Confiner = VirtualPlayerCamera.GetComponent<CinemachineConfiner>();
        }

        public void SetLocation(int LocationIndex, int SpawnPointIndex, int ConfinerIndex, int MusicIndex = 0)
        {
            var originLocation = LocationTransitions[LocationIndex].OriginLocation;
            var targetLocation = LocationTransitions[LocationIndex].TargetLocation;
            var confiner = Confiners[ConfinerIndex];
            var music = Music[MusicIndex];

            for (int i = 0; i < originLocation.Count; i++)
            {
                originLocation[i].SetActive(false);
            }

            for (int i = 0; i < targetLocation.Count; i++)
            {
                targetLocation[i].SetActive(true);
            }

            Player.Position = SpawnPoints[SpawnPointIndex].position;
            confiner.gameObject.SetActive(true);
            Confiner.m_BoundingShape2D = confiner;

            AudioManager.PlayMusic(music);
        }
    }
}