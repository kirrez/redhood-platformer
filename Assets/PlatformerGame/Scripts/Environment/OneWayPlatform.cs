using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class OneWayPlatform : BasePlatform
    {
        private IPlayer Player;

        [SerializeField]
        private Collider2D Collider;

        private void Start()
        {
            Player = CompositionRoot.GetPlayer();
        }

        protected override void FixedUpdate()
        {
            if (!IsActive) return;

            Timer -= Time.fixedDeltaTime;

            if (Collider.bounds.Contains(Player.Position))
            {
                Inside = true;
            }

            if (!Collider.bounds.Contains(Player.Position))
            {
                Inside = false;
            }

            if (Timer <= 0f && !Inside)
            {
                gameObject.layer = (int)Layers.OneWay;
                IsActive = false;
            }
        }

    }
}