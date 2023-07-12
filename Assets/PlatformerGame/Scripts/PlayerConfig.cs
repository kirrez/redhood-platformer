using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerConfig
    {
        public float HorizontalSpeed { get; private set; }
        public float CrouchSpeed { get; private set; }
        public float PushDownForce { get; private set; }
        public float JumpForce { get; private set; }
        public float RollDownForce { get; private set; }

        public float PrimaryAttackCooldown { get; private set; }
        public float JumpDownTime { get; private set; }
        public float RollDownTime { get; private set; }
        public float DeathShockTime { get; private set; }

        public float KnifeCooldown { get; private set; }
        public float AxeCooldown { get; private set; }
        public float HolyWaterCooldown { get; private set; }

        public PlayerConfig()
        {
            HorizontalSpeed    = 300f;
            CrouchSpeed        = 175f;
            PushDownForce      = 50f;
            JumpForce          = 360f; //350
            RollDownForce      = 310f;

            PrimaryAttackCooldown = 0.5f;

            JumpDownTime = 0.4f;
            RollDownTime = 0.65f;
            DeathShockTime = 1.5f;

            KnifeCooldown = 0.5f;
            AxeCooldown = 0.5f;
            HolyWaterCooldown = 1.5f;
        }
    }
}