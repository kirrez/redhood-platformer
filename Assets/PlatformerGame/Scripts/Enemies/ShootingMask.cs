using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ShootingMask : MonoBehaviour
    {
        [SerializeField]
        private Transform FirePoint;

        [SerializeField]
        private Vector2 ShotDirection;

        [SerializeField]
        private float Delay;

        [SerializeField]
        private float StartDelay;

        private float DeltaX = 15f;
        private float DeltaY = 13f;

        private float Timer;

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            AudioManager = CompositionRoot.GetAudioManager();
            ResourceManager = CompositionRoot.GetResourceManager();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
        }

        private void OnEnable()
        {
            Timer = StartDelay;
        }

        //private void OnDisable()
        //{
        //    Player = null;
        //}

        private void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = Delay;

                var instance = ResourceManager.GetFromPool(Enemies.MaskBulletRed);
                DynamicsContainer.AddEnemy(instance);
                instance.transform.position = FirePoint.position;
                instance.GetComponent<Rigidbody2D>().velocity = ShotDirection;

                if (AudibleDistance() == true) AudioManager.PlaySound(ESounds.Hit7);
            }
        }

        private bool AudibleDistance()
        {
            if (Player == null) return false;

            var distanceX = Mathf.Abs(transform.position.x - Player.Position.x);
            var distanceY = Mathf.Abs(transform.position.y - Player.Position.y);

            if (distanceX <= DeltaX && distanceY <= DeltaY) return true;

            return false;
        }
    }
}