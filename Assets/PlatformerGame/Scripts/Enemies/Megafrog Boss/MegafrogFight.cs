using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Platformer
{
    public class MegafrogFight : MonoBehaviour
    {
        [SerializeField]
        private Collider2D StartTrigger;

        [SerializeField]
        private Collider2D ArenaTrigger;

        [SerializeField]
        private Transform CameraAnchor;

        [SerializeField]
        private GameObject Blocks;

        [SerializeField]
        private Megafrog Megafrog;

        private IPlayer Player;
        private IProgressManager ProgressManager;
        private CinemachineVirtualCamera PlayerCamera;
        private CinemachineVirtualCamera BossCamera;

        private bool InsideStartTrigger;
        //private bool BossDefeated;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            PlayerCamera = CompositionRoot.GetVirtualPlayerCamera();
            BossCamera = CompositionRoot.GetStaticBossCamera();
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            Blocks.SetActive(false);
            InsideStartTrigger = false;
            if (ProgressManager.GetQuest(EQuest.Megafrog) == 0)
            {
                //BossDefeated = false;
            }
        }

        private void Update()
        {
            if (ProgressManager.GetQuest(EQuest.Megafrog) == 1) return;

            if (StartTrigger.bounds.Contains(Player.Position) && !InsideStartTrigger)
            {
                StartBattle();
            }

            if (InsideStartTrigger && !ArenaTrigger.bounds.Contains(Player.Position))
            {
                LeaveBattle();
            }
        }

        private void StartBattle()
        {
            var game = CompositionRoot.GetGame();

            InsideStartTrigger = true;
            Blocks.SetActive(true);
            BossCamera.m_Priority = 15;
            BossCamera.Follow = CameraAnchor;

            Megafrog.Initiate(this, ProgressManager.GetQuest(EQuest.MegafrogMaxHealth));

            game.HUD.SetEnemyIcon(0);
            game.HUD.ShowEnemyHealthBar();
            game.HUD.SetEnemyMaxHealth(ProgressManager.GetQuest(EQuest.MegafrogMaxHealth));
        }

        private void LeaveBattle()
        {
            var game = CompositionRoot.GetGame();

            InsideStartTrigger = false;
            Blocks.SetActive(false);
            BossCamera.m_Priority = 5;
            PlayerCamera.Follow = Player.Transform;

            game.HUD.HideEnemyHealthBar();
        }

        public void WinBattle()
        {
            ProgressManager.SetQuest(EQuest.Megafrog, 1);
            LeaveBattle();
        }
    }
}