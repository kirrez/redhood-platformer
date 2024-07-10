using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Platformer
{
    public class MegafrogFight : MonoBehaviour
    {
        [SerializeField]
        private Collider2D Trigger;

        [SerializeField]
        private Collider2D ArenaTrigger;

        [SerializeField]
        private Transform CameraAnchor;

        [SerializeField]
        private GameObject Blocks;

        [SerializeField]
        private Megafrog Megafrog;

        [SerializeField]
        private Stage Stage;

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IProgressManager ProgressManager;

        private CinemachineVirtualCamera PlayerCamera;
        private CinemachineVirtualCamera BossCamera;

        private bool Inside;
        private bool IsFighting;

        private delegate void State();
        private State CurrentState = () => { };

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            AudioManager = CompositionRoot.GetAudioManager();
            ProgressManager = CompositionRoot.GetProgressManager();

            PlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            BossCamera = CompositionRoot.GetStaticBossCamera();
        }

        private void OnEnable()
        {
            Blocks.SetActive(false);
            Inside = false;

            var quest = ProgressManager.GetQuest(EQuest.Megafrog);

            if (quest == 0)
            {
                CurrentState = StateCheckStart;
            }

            if (quest == 1)
            {
                CurrentState = () => { };
            }
        }

        private void Update()
        {
            CurrentState();
        }

        private void OnDisable()
        {
            if (IsFighting == true)
            {
                LeaveBattle();
            }
        }

        private void StateCheckStart()
        {
            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                IsFighting = true;
                Blocks.SetActive(true);
                BossCamera.m_Priority = 15;
                BossCamera.Follow = CameraAnchor;

                Megafrog.Initiate(this, ProgressManager.GetQuest(EQuest.MegafrogMaxHealth));

                Game.HUD.SetEnemyIcon(0);
                Game.HUD.ShowEnemyHealthBar();
                Game.HUD.SetEnemyMaxHealth(ProgressManager.GetQuest(EQuest.MegafrogMaxHealth));

                AudioManager.PlayMusic(Stage.Music[1]);
                IsFighting = true;

                CurrentState = () => { };
            }
        }

        private void LeaveBattle()
        {
            var game = CompositionRoot.GetGame();

            Inside = false;
            Blocks.SetActive(false);
            BossCamera.m_Priority = 5;
            PlayerCamera.Follow = Player.Transform;

            Game.HUD.HideEnemyHealthBar();
            AudioManager.PlayMusic(Stage.Music[0]);
        }

        public void WinBattle()
        {
            ProgressManager.SetQuest(EQuest.Megafrog, 1);
            LeaveBattle();
        }
    }
}