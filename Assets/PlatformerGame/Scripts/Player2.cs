using Platformer.ScriptedAnimator;
using Platformer.PlayerStates;
using UnityEngine;
using System;

namespace Platformer
{
    public class Player2 : MonoBehaviour, IPlayer
    {
        public Health Health { get; private set; }
        public IPlayerAnimations Animations { get; private set; }

        public event Action Interaction = () => { };

        public Transform Transform => transform;

        public Rigidbody2D Body
        { 
            get { return Rigidbody; }
            set { Rigidbody = value; }
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        //Input
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
        public bool HitJump { get; set; }
        public bool HitAttack { get; set; }

        public bool HitInteraction { get; set; }
        public float DeltaY { get; private set; }
        //public bool InAir { get; set; }

        public float JumpDownTime { get; private set; }
        public float RollDownTime { get; private set; }
        public float DeathShockTime { get; private set; }

        private StateIdle StateIdle;
        private StateWalk StateWalk;
        private StateDying StateDying;
        private StateAttack StateAttack;
        private StateJumpRising StateJumpRising;
        private StateJumpFalling StateJumpFalling;
        private StateJumpRisingAttack StateJumpRisingAttack;
        private StateJumpFallingAttack StateJumpFallingAttack;
        private StateJumpDown StateJumpDown;
        private StateSit StateSit;
        private StateSitAttack StateSitAttack;
        private StateSitCrouch StateSitCrouch;
        private StateRollDown StateRollDown;
        private StateDamageTaken StateDamageTaken;
        private StateSitDamageTaken StateSitDamageTaken;
        private StateInteraction StateInteraction;
        private StateStunned StateStunned;

        private BaseState CurrentState;

        private Rigidbody2D Rigidbody;
        private SpriteRenderer Renderer;
        private PlayerAnimator PlayerAnimator;

        private float DirectionX = 1f;

        private Vector3 LastPosition;

        private PlayerConfig Config;

        private float HorizontalSpeed;
        private float CrouchSpeed;
        private float PushDownForce;
        private float JumpForce;
        private float RollDownForce;

        //Two paths for collider to switch between while change StandUp and SitDown ))
        [SerializeField]
        private Vector2[] StandingPath;

        [SerializeField]
        private Vector2[] SittingPath;

        private PolygonCollider2D Collider;
        private bool IsSitting;

        // link to a moving platform under player's feet
        private Rigidbody2D PlatformRigidbody = null;
        private BasePlatform PlatformInstance = null;

        //Weapons
        [SerializeField]
        private Transform StandingFirePoint;
        [SerializeField]
        private Transform SittingFirePoint;

        private Transform FirePoint;
        private float StandingFirePointX;
        private float SittingFirePointX;

        private float KnifeTimer;
        private float AxeTimer;
        private float HolyWaterTimer;

        private float KnifeCooldown;
        private float AxeCooldown;
        private float HolyWaterCooldown;

        private int KnifeLevel;
        private int AxeLevel;
        private int HolyWaterLevel;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private IGame Game;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();

            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            Health = GetComponent<Health>();

            Collider = GetComponent<PolygonCollider2D>();

            PlayerAnimator = new PlayerAnimator(Renderer);
            Animations = new PlayerAnimation(PlayerAnimator);

            StateIdle = new StateIdle(this);
            StateWalk = new StateWalk(this);
            StateDying = new StateDying(this);
            StateAttack = new StateAttack(this);
            StateJumpRising = new StateJumpRising(this);
            StateJumpFalling = new StateJumpFalling(this);
            StateJumpRisingAttack = new StateJumpRisingAttack(this);
            StateJumpFallingAttack = new StateJumpFallingAttack(this);
            StateJumpDown = new StateJumpDown(this);
            StateSit = new StateSit(this);
            StateSitAttack = new StateSitAttack(this);
            StateSitCrouch = new StateSitCrouch(this);
            StateRollDown = new StateRollDown(this);
            StateDamageTaken = new StateDamageTaken(this);
            StateSitDamageTaken = new StateSitDamageTaken(this);
            StateInteraction = new StateInteraction(this);
            StateStunned = new StateStunned(this);

            Config = new PlayerConfig();
        }

        public void Initiate(IGame game)
        {
            Game = game;
        }

        private void OnEnable()
        {
            LoadConfigData();
            StandingFirePointX = StandingFirePoint.transform.localPosition.x;
            SittingFirePointX = SittingFirePoint.transform.localPosition.x;
        }

        private void Update()
        {
            CurrentState.Update(); //GetInput inside

            PlayerAnimator.Update(); //Time.deltaTime on timer inside 
        }


        private void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        public void SetDeltaY()
        {
            DeltaY = transform.position.y - LastPosition.y;
            LastPosition = transform.position;
        }

        public void UpdateAttackTimers()
        {
            var delta = Time.fixedDeltaTime;

            if (KnifeTimer > 0) KnifeTimer -= delta;
            if (AxeTimer > 0) AxeTimer -= delta;
            if (HolyWaterTimer > 0) HolyWaterTimer -= delta;
        }

        public void UpdateMaxLives()
        {
            var maxLives = ProgressManager.GetQuest(EQuest.MaxLives);
            Health.SetMaxLives(maxLives);
            Health.RefillHealth();

            Game.HUD.SetMaxLives(maxLives);
            Game.HUD.SetCurrentLives(maxLives);
        }

        public void Revive()
        {
            UpdateAllWeaponLevel();
            Game.HUD.UpdateWeaponIcons();
            UpdateMaxLives();
            InactivateCollider(false);

            SetState(EPlayerStates.Idle, 0f);
            Animations.Idle();

            StandUp();

            SetPlayerDirection(true);
        }

        public void SetState(EPlayerStates state, float time = 0f)
        {
            switch (state)
            {
                case EPlayerStates.Idle:
                    CurrentState = StateIdle;
                    break;
                case EPlayerStates.Walk:
                    CurrentState = StateWalk;
                    break;
                case EPlayerStates.Attack:
                    CurrentState = StateAttack;
                    break;
                case EPlayerStates.JumpRising:
                    CurrentState = StateJumpRising;
                    break;
                case EPlayerStates.JumpRisingAttack:
                    CurrentState = StateJumpRisingAttack;
                    break;
                case EPlayerStates.JumpFalling:
                    CurrentState = StateJumpFalling;
                    break;
                case EPlayerStates.JumpFallingAttack:
                    CurrentState = StateJumpFallingAttack;
                    break;
                case EPlayerStates.JumpDown:
                    CurrentState = StateJumpDown;
                    break;
                case EPlayerStates.RollDown:
                    CurrentState = StateRollDown;
                    break;
                case EPlayerStates.Sit:
                    CurrentState = StateSit;
                    break;
                case EPlayerStates.SitCrouch:
                    CurrentState = StateSitCrouch;
                    break;
                case EPlayerStates.SitAttack:
                    CurrentState = StateSitAttack;
                    break;
                case EPlayerStates.SitDamageTaken:
                    CurrentState = StateSitDamageTaken;
                    break;
                case EPlayerStates.DamageTaken:
                    CurrentState = StateDamageTaken;
                    break;
                case EPlayerStates.Dying:
                    CurrentState = StateDying;
                    break;
                case EPlayerStates.Interaction:
                    CurrentState = StateInteraction;
                    break;
                case EPlayerStates.Stunned:
                    CurrentState = StateStunned;
                    break;
            }
            CurrentState.OnEnable(time);
        }

        // also returns "1" if player faces right and "-1" if left )) for external checks in spawners etc.
        public float DirectionCheck()
        {
            // Changes Renderer and weapon's directions
            if (Horizontal > 0)
            {
                SetPlayerDirection(true);
                return 1f;
            }
            if (Horizontal < 0)
            {
                SetPlayerDirection(false);
                return -1f;
            }
            return DirectionX;
        }

        private void SetPlayerDirection(bool faceRight)
        {
            Vector2 newPosition;

            Renderer.flipX = !faceRight;

            if (faceRight == true)
            {
                DirectionX = 1f;
            }

            if (faceRight == false)
            {
                DirectionX = -1f;
            }

            newPosition = new Vector2(StandingFirePointX * DirectionX, StandingFirePoint.localPosition.y);
            StandingFirePoint.localPosition = newPosition;

            newPosition = new Vector2(SittingFirePointX * DirectionX, SittingFirePoint.localPosition.y);
            SittingFirePoint.localPosition = newPosition;
        }

        // for Double Jump??
        //public void UpdateInAir(bool state)
        //{
        //    InAir = state;
        //}

        public void Walk()
        {
            if (PlatformRigidbody != null)
            {
                Rigidbody.velocity = new Vector2(Horizontal * Time.fixedDeltaTime * HorizontalSpeed, 0f) + PlatformRigidbody.velocity;
            }

            if (PlatformRigidbody == null)
            {
                Rigidbody.velocity = new Vector2(Horizontal * Time.fixedDeltaTime * HorizontalSpeed, Rigidbody.velocity.y);
            }
        }

        public void Crouch()
        {
            if (PlatformRigidbody != null)
            {
                Rigidbody.velocity = new Vector2(Horizontal * Time.fixedDeltaTime * CrouchSpeed, 0f) + PlatformRigidbody.velocity;
            }

            if (PlatformRigidbody == null)
            {
                Rigidbody.velocity = new Vector2(Horizontal * Time.fixedDeltaTime * CrouchSpeed, Rigidbody.velocity.y);
            }
        }

        // For Idle and Sit States while riding a platform
        public void StickToPlatform()
        {
            if (PlatformRigidbody == null) return;
            Rigidbody.velocity = PlatformRigidbody.velocity;
        }

        // For Idle state on steep slopes, to prevent slip
        public void ResetVelocity()
        {
            Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly
        }

        // vertical movement for correcting height in JumpRising state
        public void PushDown()
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y + PushDownForce * Time.fixedDeltaTime * (-1));
        }

        public void Jump()
        {
            Rigidbody.AddForce(new Vector2(0f, JumpForce));
        }

        public void DamagePushBack()
        {
            // direction from Health ))
            float direction = Health.DamageDirection;
            
            if (direction != 0)
            {
                Horizontal = direction;
            }

            if (direction == 0)
            {
                direction = DirectionX * -1f;
                Horizontal = direction;
            }

            DirectionCheck();
            // magic numbers, no need to take out into config.. 2.3f / 1.75f
            Rigidbody.AddForce(new Vector2(HorizontalSpeed / 2.3f * direction, JumpForce / 1.75f));
        }

        public void RollDown()
        {
            Rigidbody.AddForce(new Vector2(DirectionX * RollDownForce, 0f));
        }

        public void StandUp()
        {
            Collider.SetPath(0, StandingPath);
            IsSitting = false;
            FirePoint = StandingFirePoint;
        }

        public void SitDown()
        {
            Collider.SetPath(0, SittingPath);
            IsSitting = true;
            FirePoint = SittingFirePoint;
        }

        public bool IsKnifeAttack()
        {
            if (KnifeTimer <= 0)
            {
                //KnifeTimer = KnifeCooldown;

                if (HitAttack && Vertical < 1f)
                {
                    KnifeTimer = KnifeCooldown;
                    HitAttack = false;
                    if (KnifeLevel > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsAxeAttack()
        {
            if (AxeTimer <= 0)
            {
                //AxeTimer = AxeCooldown;

                if (HitAttack && Vertical == 1f)
                {
                    AxeTimer = AxeCooldown;
                    HitAttack = false;
                    if (AxeLevel > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsHolyWaterAttack()
        {
            if (HolyWaterTimer <= 0)
            {
                //HolyWaterTimer = HolyWaterCooldown;

                if (HitInteraction && Vertical == 1)
                {
                    HolyWaterTimer = HolyWaterCooldown;
                    HitInteraction = false;
                    if (HolyWaterLevel > 0)
                    {
                        return true;
                    }
                }
            }
            HitInteraction = false;

            return false;
        }

        public void ShootKnife()
        {
            var knifeLevel = ProgressManager.GetQuest(EQuest.KnifeLevel);
            GameObject instance = null;

            switch (knifeLevel)
            {
                case 1:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.KitchenKnife);
                    break;
                case 2:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.FarmerKnife);
                    break;
                case 3:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.HunterKnife);
                    break;
            }

            //all cases
            if (knifeLevel > 0)
            {
                DynamicsContainer.AddMain(instance);

                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= DirectionX;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;
                instance.GetComponent<IBaseGFX>().Initiate(FirePoint.position, DirectionX);

                AudioManager.PlayRedhoodSound(EPlayerSounds.ThrowKnife);
            }
        }

        public void ShootAxe()
        {
            var axeLevel = ProgressManager.GetQuest(EQuest.AxeLevel);
            GameObject instance = null;

            switch (axeLevel)
            {
                case 1:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.CrippledAxe);
                    break;
                case 2:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.SharpenedAxe);
                    break;
                case 3:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.SturdyAxe);
                    break;
            }

            //all cases
            if (axeLevel > 0)
            {
                DynamicsContainer.AddMain(instance);

                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= DirectionX;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;
                instance.GetComponent<IBaseGFX>().Initiate(FirePoint.position, DirectionX);

                AudioManager.PlayRedhoodSound(EPlayerSounds.ThrowAxe);
            }

        }

        public void ShootHolyWater()
        {
            var holyWaterLevel = ProgressManager.GetQuest(EQuest.HolyWaterLevel);
            GameObject instance = null;

            switch (holyWaterLevel)
            {
                case 1:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.WeakHolyWater);
                    break;
                case 2:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.StrongHolyWater);
                    break;
            }

            //all cases
            if (holyWaterLevel > 0)
            {
                DynamicsContainer.AddMain(instance);

                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= DirectionX;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;
                instance.GetComponent<IBaseGFX>().Initiate(FirePoint.position, DirectionX);

                AudioManager.PlayRedhoodSound(EPlayerSounds.ThrowBottle);
            }
        }

        // used in all Sit-states to prevent standing up while in low-heighted tunnel
        public bool Ceiled(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y + Collider.bounds.extents.y);
            var boxSize = new Vector2(Collider.bounds.size.x, 0.15f);
            float distance = 0.9f; // Magic number, height of standing collider(1.8f) - height of sitting(0.9f);

            RaycastHit2D CeilHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.up, distance, mask);

            //true only while sitting )
            //return (CeilHit.collider != null) && (SittingCollider.enabled);
            return (CeilHit.collider != null) && IsSitting;
        }

        // used in Idle and Walk states for preventing jump under too low ceil (solid objects)
        public bool StandingCeiled(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y + Collider.bounds.extents.y);
            var boxSize = new Vector2(Collider.bounds.size.x, 0.15f);
            float distance = 0.3f; // Magic number, height of standing collider(1.8f) - height of sitting(0.9f);

            RaycastHit2D CeilHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.up, distance, mask);

            return CeilHit.collider != null;
        }

        // mostly used for checking ability to jump
        public bool Grounded(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var boxSize = new Vector2(Collider.bounds.size.x, 0.1f);

            //float distance = 0.05f; // Magic number, empirical
            float distance = 0.1f;

            RaycastHit2D GroundHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, distance, mask);

            //catching PlatformInstance
            if (mask == LayerMasks.OneWay)
            {
                if (GroundHit.collider != null)
                {
                    PlatformInstance = GroundHit.collider.gameObject.GetComponent<OneWayPlatform>();
                }

                if (GroundHit.collider == null)
                {
                    PlatformInstance = null;
                }

            }

            // check moving platform and catching its PlatformInstance
            if (mask == LayerMasks.PlatformOneWay)
            {
                if (GroundHit.collider != null)
                {
                    PlatformRigidbody = GroundHit.collider.gameObject.GetComponent<Rigidbody2D>();
                    if (mask == LayerMasks.PlatformOneWay)
                    {
                        PlatformInstance = GroundHit.collider.gameObject.GetComponent<MovingPlatform>();
                    }
                }

                if (GroundHit.collider == null)
                {
                    PlatformRigidbody = null;
                    if (mask == LayerMasks.PlatformOneWay)
                    {
                        PlatformInstance = null;
                    }
                }
            }

            return GroundHit.collider != null;
        }

        public void ReleasePlatform()
        {
            PlatformRigidbody = null;
        }

        public void ResetDamageCooldown()
        {
            Health.ResetDamageCooldown();
        }

        public void EnableGameOver()
        {
            Game.GameOverMenu();
        }

        public void JumpDown()
        {
            if (PlatformInstance != null)
            {
                PlatformInstance.ComeThrough();
                //?release platform
                ReleasePlatform();
            }
        }

        public void InactivateCollider(bool flag)
        {
            if (flag)
            {
                this.gameObject.layer = (int)Layers.InactivePlayer;
            }
            
            if (!flag)
            {
                this.gameObject.layer = (int)Layers.FeetCollider;
            }
        }

        public void UpdateStateName(string name)
        {
            //Hud.ChangeStateName(name);
        }

        public void ChangeHealthUI()
        {
            int lives = Health.GetHitPoints;
            Game.HUD.SetCurrentLives(lives);
        }

        public void GetInput()
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");

            if (HitJump && Input.GetButtonUp("Jump"))
            {
                HitJump = false;
            }

            if (!HitJump && Input.GetButtonDown("Jump"))
            {
                HitJump = true;
            }

            if (!HitAttack && Input.GetButtonDown("Fire1"))
            {
                HitAttack = true;
            }

            GetInteractionInput();

        }

        public void GetInteractionInput()
        {
            //for interaction with objects and dialogues
            if (Input.GetButtonDown("Fire2"))
            {
                Interaction();
            }

            if (!HitInteraction && Input.GetButtonDown("Fire2"))
            {
                HitInteraction = true;
            }
        }

        public void HoldByInteraction()
        {
            SetState(EPlayerStates.Interaction);
        }

        public void ReleasedByInteraction()
        {
            SetState(EPlayerStates.Idle);
            Animations.Idle();
            InactivateCollider(false);
        }

        public void GetStunned(float time)
        {
            SetState(EPlayerStates.Stunned, time);
        }

        public void UpdateAllWeaponLevel()
        {
            KnifeLevel = ProgressManager.GetQuest(EQuest.KnifeLevel);
            AxeLevel = ProgressManager.GetQuest(EQuest.AxeLevel);
            HolyWaterLevel = ProgressManager.GetQuest(EQuest.HolyWaterLevel);
        }

        private void LoadConfigData()
        {
            HorizontalSpeed = Config.HorizontalSpeed;
            CrouchSpeed = Config.CrouchSpeed;
            PushDownForce = Config.PushDownForce;
            JumpForce = Config.JumpForce;
            RollDownForce = Config.RollDownForce;
            DeathShockTime = Config.DeathShockTime;
            RollDownTime = Config.RollDownTime;
            JumpDownTime = Config.JumpDownTime;

            KnifeCooldown = Config.KnifeCooldown;
            AxeCooldown = Config.AxeCooldown;
            HolyWaterCooldown = Config.HolyWaterCooldown;
        }
    }
}