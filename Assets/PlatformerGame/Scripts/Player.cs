using Platformer.ScriptedAnimator;
using Platformer.PlayerStates;
using UnityEngine;
using System;

namespace Platformer
{
    public class Player : MonoBehaviour, IPlayer
    {
        public EFacing Facing { get; private set; } = EFacing.Right;

        public event Action Interaction = () => { };

        public Transform Transform => transform;

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Health Health;
        public IPlayerAnimations Animations;

        //Input
        public float Horizontal;
        public float Vertical;
        public bool HitJump;
        public bool HitAttack;
        public bool HitInteraction;

        public float DeltaY { get; private set; }
        public float JumpDownTime { get; private set; }
        public float RollDownTime { get; private set; }
        public float DeathShockTime { get; private set; }

        public StateIdle StateIdle;
        public StateWalk StateWalk;
        public StateDying StateDying;
        public StateAttack StateAttack;
        public StateJumpRising StateJumpRising;
        public StateJumpFalling StateJumpFalling;
        //public StateJumpRisingAttack StateJumpRisingAttack;
        //public StateJumpFallingAttack StateJumpFallingAttack;
        //public StateJumpDown StateJumpDown;
        //public StateSit StateSit;
        //public StateSitAttack StateSitAttack;
        //public StateSitCrouch StateSitCrouch;
        //public StateRollDown StateRollDown;
        //public StateDamageTaken StateDamageTaken;
        //public StateSitDamageTaken StateSitDamageTaken;
        //public StateInteraction StateInteraction;
        //public StateStunned StateStunned;

        private IState CurrentState;

        public Rigidbody2D Rigidbody;
        public SpriteRenderer Renderer;
        public PlayerAnimator PlayerAnimator;

        public Vector3 LastPosition;
        public PlayerConfig Config;

        public float HorizontalSpeed;
        public float CrouchSpeed;
        public float PushDownForce;
        public float JumpForce;
        public float RollDownForce;

        public float Timer;

        //Two paths for collider to switch between while change StandUp and SitDown ))
        [SerializeField]
        private Vector2[] StandingPath;
        [SerializeField]
        private Vector2[] SittingPath;
        private PolygonCollider2D Collider;
        private bool IsSitting;
        // link to a moving platform under player's feet
        public Rigidbody2D PlatformRigidbody = null;
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

        private IGame Game;
        private IResourceManager ResourceManager;
        private IProgressManager ProgressManager;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            ProgressManager = CompositionRoot.GetProgressManager();

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
            //StateJumpRisingAttack = new StateJumpRisingAttack(this);
            //StateJumpFallingAttack = new StateJumpFallingAttack(this);
            //StateJumpDown = new StateJumpDown(this);
            //StateSit = new StateSit(this);
            //StateSitAttack = new StateSitAttack(this);
            //StateSitCrouch = new StateSitCrouch(this);
            //StateRollDown = new StateRollDown(this);
            //StateDamageTaken = new StateDamageTaken(this);
            //StateSitDamageTaken = new StateSitDamageTaken(this);
            //StateInteraction = new StateInteraction(this);
            //StateStunned = new StateStunned(this);

            Config = new PlayerConfig();
        }

        public void Initiate(IGame game)
        {
            Game = game;
        }

        private void OnEnable()
        {
            LoadConfigData();
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

            SetState(StateIdle, 0f);
            Animations.Idle();
            StandUp();

            StandingFirePointX = StandingFirePoint.transform.localPosition.x;
            SittingFirePointX = SittingFirePoint.transform.localPosition.x;
        }

        public void SetState(IState state, float time = 0f)
        {
            CurrentState = state;

            CurrentState.OnEnable(time);
        }

        public void UpdateFacing()
        {
            // Changes Renderer and weapon's directions
            if (Horizontal > 0)
            {
                Renderer.flipX = false;
                Facing = EFacing.Right;

                var newPosition = new Vector2(StandingFirePointX, StandingFirePoint.localPosition.y);
                StandingFirePoint.localPosition = newPosition;

                newPosition = new Vector2(SittingFirePointX, SittingFirePoint.localPosition.y);
                SittingFirePoint.localPosition = newPosition;
            }

            if (Horizontal < 0)
            {
                Renderer.flipX = true;
                Facing = EFacing.Left;

                var newPosition = new Vector2(-StandingFirePointX, StandingFirePoint.localPosition.y);
                StandingFirePoint.localPosition = newPosition;

                newPosition = new Vector2(-SittingFirePointX, SittingFirePoint.localPosition.y);
                SittingFirePoint.localPosition = newPosition;
            }
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
            if (KnifeTimer > 0)
            {
                return false;
            }

            if (HitAttack && Vertical < 1f)
            {
                KnifeTimer = KnifeCooldown;
                HitAttack = false;

                if (KnifeLevel > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAxeAttack()
        {
            if (AxeTimer > 0)
            {
                return false;
            }

            if (HitAttack && Vertical == 1f)
            {
                AxeTimer = AxeCooldown;
                HitAttack = false;

                if (AxeLevel > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsHolyWaterAttack()
        {
            if (HolyWaterTimer > 0)
            {
                HitInteraction = false;
                return false;
            }

            if (HitInteraction && Vertical == 1)
            {
                HolyWaterTimer = HolyWaterCooldown;
                HitInteraction = false;

                if (HolyWaterLevel > 0)
                {
                    return true;
                }
            }

            HitInteraction = false;
            return false;
        }

        public void ShootKnife()
        {
            var knife = ProgressManager.GetQuest(EQuest.KnifeLevel);
            var dynamics = CompositionRoot.GetDynamicsContainer();

            GameObject instance = null;

            switch (knife)
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
            if (knife > 0)
            {
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);

                var x = Facing == EFacing.Right ? 1 : -1;
                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= x;

                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

                var weapon = instance.GetComponent<PlayerKnife>();
                weapon.Initiate(FirePoint.position, Facing);
            }
        }

        public void ShootAxe()
        {
            var axe = ProgressManager.GetQuest(EQuest.AxeLevel);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            GameObject instance = null;

            switch (axe)
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
            if (axe > 0)
            {
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);

                var x = Facing == EFacing.Right ? 1 : -1;
                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= x;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

                var weapon = instance.GetComponent<PlayerAxe>();
                weapon.Initiate(FirePoint.position, Facing);
            }
        }

        public void ShootHolyWater()
        {
            var holyWater = ProgressManager.GetQuest(EQuest.HolyWaterLevel);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            GameObject instance = null;
            GameObject disappearEffect = null;

            switch (holyWater)
            {
                case 1:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.WeakHolyWater);
                    break;
                case 2:
                    instance = ResourceManager.GetFromPool(EPlayerWeapons.StrongHolyWater);
                    break;
            }

            //all cases
            if (holyWater > 0)
            {
                instance.transform.SetParent(dynamics.Transform, false);
                dynamics.AddItem(instance);

                var x = Facing == EFacing.Right ? 1 : -1;
                var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
                weaponVelocity.x *= x;
                instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

                var weapon = instance.GetComponent<PlayerHolyWaterBottle>();
                weapon.Initiate(FirePoint.position, Facing);

                weapon.Disappear += delegate (Transform trans)
                {
                    disappearEffect = ResourceManager.GetFromPool(GFXs.HolyWaterDisappear);
                    dynamics.AddItem(disappearEffect);
                    disappearEffect.transform.SetParent(dynamics.Transform, false);
                    disappearEffect.transform.position = trans.position;
                };
                
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
            //SetState(StateInteraction);
        }

        public void ReleasedByInteraction()
        {
            SetState(StateIdle);
            Animations.Idle();
            InactivateCollider(false);
        }

        public void Stun(float time)
        {
            //SetState(StateStunned, time);
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

        private void UpdateAllWeaponLevel()
        {
            var progressManager = CompositionRoot.GetProgressManager();
            KnifeLevel = ProgressManager.GetQuest(EQuest.KnifeLevel);
            AxeLevel = ProgressManager.GetQuest(EQuest.AxeLevel);
            HolyWaterLevel = ProgressManager.GetQuest(EQuest.HolyWaterLevel);
        }

    }
}