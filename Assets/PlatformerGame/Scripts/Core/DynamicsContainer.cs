using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DynamicsContainer : MonoBehaviour, IDynamicsContainer
    {
        [SerializeField]
        private Transform EnemiesContainer;

        [SerializeField]
        private Transform MainContainer;

        [SerializeField]
        private Transform MusicContainer;

        [SerializeField]
        private Transform SoundsContainer;

        [SerializeField]
        private Transform TemporaryContainer;

        public Transform Enemies => EnemiesContainer;
        public Transform Main => MainContainer;
        public Transform Music => MusicContainer;
        public Transform Sounds => SoundsContainer;
        public Transform Temporary => TemporaryContainer;

        private List<GameObject> EnemiesContent;
        private List<GameObject> MainContent;
        private List<GameObject> MusicContent;
        private List<GameObject> SoundsContent;
        private List<GameObject> TemporaryContent;
        private List<GameObject> CampFiresContent;

        public void AddEnemy(GameObject item)
        {
            if (!EnemiesContent.Contains(item))
            {
                EnemiesContent.Add(item);
                item.transform.SetParent(Enemies, false);
            }
        }

        public void AddMain(GameObject item)
        {
            if (!MainContent.Contains(item))
            {
                MainContent.Add(item);
                item.transform.SetParent(Main, false);
            }
        }

        public void AddMusic(GameObject item)
        {
            if (!MusicContent.Contains(item))
            {
                MusicContent.Add(item);

                item.transform.SetParent(Music, false);
            }
        }

        public void AddSound(GameObject item)
        {
            if (!SoundsContent.Contains(item))
            {
                SoundsContent.Add(item);

                item.transform.SetParent(Sounds, false);
            }
        }

        public void AddTemporary(GameObject item)
        {
            if (!TemporaryContent.Contains(item))
            {
                TemporaryContent.Add(item);

                item.transform.SetParent(Temporary, false);
            }
        }

        public void DeactivateEnemies()
        {
            foreach (var item in EnemiesContent)
            {
                item.SetActive(false);
            }
        }

        public void DeactivateMain()
        {
            foreach (var item in MainContent)
            {
                item.SetActive(false);
            }
        }

        public void DeactivateTemporary()
        {
            foreach (var item in TemporaryContent)
            {
                item.SetActive(false);
            }
        }

        private void Awake()
        {
            EnemiesContent = new List<GameObject>();
            MainContent = new List<GameObject>();
            MusicContent = new List<GameObject>();
            SoundsContent = new List<GameObject>();
            TemporaryContent = new List<GameObject>();
            CampFiresContent = new List<GameObject>();
        }
    }
}