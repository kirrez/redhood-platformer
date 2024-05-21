using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TheVillageCursedSecret : MonoBehaviour
    {
        [SerializeField]
        private GameObject FirstBlock;

        [SerializeField]
        private GameObject SecondBlock;

        [SerializeField]
        private GameObject OneWay;

        [SerializeField]
        private GhostWall Wall;

        [SerializeField]
        private Collider2D Trigger;

        private bool Inside;
        private int WaterLevel;
        private int Progress;

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            WaterLevel = ProgressManager.GetQuest(EQuest.HolyWaterLevel);
            Progress = ProgressManager.GetQuest(EQuest.TheVillageCursedSecret);

            //Checking start condition
            if (WaterLevel == 0)
            {
                FirstBlock.SetActive(true);
                Wall.gameObject.SetActive(false);
            }

            if (WaterLevel > 0)
            {
                FirstBlock.SetActive(false);
                Wall.gameObject.SetActive(true);
                Wall.Activate();
            }

            //checking completion
            if (Progress == 0)
            {
                Inside = false;
                SecondBlock.SetActive(true);
                OneWay.SetActive(false);
            }

            if (Progress > 0)
            {
                SecondBlock.SetActive(false);
                OneWay.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            if (Trigger.bounds.Contains(Player.Position) && Progress == 0 && Inside == false)
            {
                Inside = true;
                ProgressManager.SetQuest(EQuest.TheVillageCursedSecret, 1);
                SecondBlock.SetActive(false);
                OneWay.SetActive(true);

                AudioManager.PlaySound(ESounds.DoorHeavy);
            }
        }


    }
}