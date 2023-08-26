using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class OneWayExtra : MonoBehaviour
    {
        [SerializeField]
        private Collider2D Collider;

        [SerializeField]
        private Collider2D Trigger;

        private IPlayer Player;

        private float Delay = 0.5f;
        private float Timer;
        private bool Inside;

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void OnEnable()
        {
            if (!Inside)
            {
                Collider.gameObject.layer = (int)Layers.OneWay;
            }
        }

        private void FixedUpdate()
        {
            if (Trigger.bounds.Contains(Player.Position))
            {
                Inside = true;
                Collider.gameObject.layer = (int)Layers.OneWayTransparent;
            }

            if (!Trigger.bounds.Contains(Player.Position))
            {
                Inside = false;
                Collider.gameObject.layer = (int)Layers.OneWay;
            }

            //if (Timer <= 0f && !Inside)
            //{
            //    gameObject.layer = (int)Layers.OneWay;
            //    IsActive = false;
            //}
        }

        public void ComeThrough()
        {
            Timer = Delay;
            Collider.gameObject.layer = (int)Layers.OneWayTransparent;
            //Debug.Log("ComeThrough");
        }
    }
}