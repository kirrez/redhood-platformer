using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerConfig
    {
        public float WalkSpeed { get; private set; }
        public float CrouchSpeed { get; private set; }
        public float PushDownSpeed { get; private set; }
        public float JumpForce { get; private set; }
        public float RollDownForce { get; private set; }
        public float PushBackForce { get; private set; }

        public float PrimaryAttackCooldown { get; private set; }
        public float JumpDownTime { get; private set; }
        public float RollDownTime { get; private set; }
        public float DeathShockTime { get; private set; }

        public float KnifeCooldown { get; private set; }
        public float AxeCooldown { get; private set; }
        public float HolyWaterCooldown { get; private set; }

        public PlayerConfig()
        {
            //HorizontalSpeed    = 300f;
            WalkSpeed      = 6f;

            //CrouchSpeed        = 175f;
            CrouchSpeed          = 3.5f;

            //PushDownSpeed      = 50f;
            PushDownSpeed        = 1.2f;

            JumpForce          = 360f; //350
            RollDownForce      = 310f;
            PushBackForce      = 130f;

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