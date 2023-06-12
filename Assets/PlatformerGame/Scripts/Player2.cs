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
        private float PrimaryAttackCooldown;

        //[SerializeField]
        //private Collider2D StandingCollider;
        //[SerializeField]
        //private Collider2D SittingCollider;
        // current collider

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

        private float AttackTimer;

        // UI, have to fix their inspector-only fill !
        //[SerializeField]
        //private HUD Hud;

        //[SerializeField]
        //private GameOverScreen GameOver;

        //[SerializeField]
        //private StartScreen StartScreen;

        private IResourceManager ResourceManager;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();

            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            Health = GetComponent<Health>();

            //New Collider reference
            Collider = GetComponent<PolygonCollider2D>();

            PlayerAnimator = new PlayerAnimator(Renderer);
            Animations = new PlayerAnimation(PlayerAnimator);

            // commands for Game model, which I don't have yet
            //GameOver.Hide();
            //StartScreen.Show();
            //Hud.Show();

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

            Config = new PlayerConfig();
        }

        private void OnEnable()
        {
            LoadConfigData();

            Health.RefillHealth();

            //now won't work
            //int lives = Health.GetHitPoints;
            //Hud.ChangeLivesAmount(lives);

            CurrentState = StateIdle;
            Animations.Idle();
            StandUp();

            StandingFirePointX = StandingFirePoint.transform.localPosition.x;
            SittingFirePointX = SittingFirePoint.transform.localPosition.x;
        }

        private void Update()
        {
            CurrentState.OnUpdate(); //GetInput inside

            PlayerAnimator.Update(); //Time.deltaTime on timer inside 
        }

        private void FixedUpdate()
        {
            DeltaY = transform.position.y - LastPosition.y;
            LastPosition = transform.position;

            CurrentState.OnFixedUpdate();

            if (AttackTimer > 0)
            {
                AttackTimer -= Time.fixedDeltaTime;
            }
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
            }
            CurrentState.Activate(time);
        }


        // also returns "1" if player faces right and "-1" if left )) for external checks in spawners etc.
        public float DirectionCheck()
        {
            Vector2 newPosition;
            // Changes Renderer and weapon's directions
            if (Horizontal > 0)
            {
                Renderer.flipX = false;
                DirectionX = 1f;

                newPosition = new Vector2(StandingFirePointX, StandingFirePoint.localPosition.y);
                StandingFirePoint.localPosition = newPosition;

                newPosition = new Vector2(SittingFirePointX, SittingFirePoint.localPosition.y);
                SittingFirePoint.localPosition = newPosition;
                return 1f;
            }
            if (Horizontal < 0)
            {
                Renderer.flipX = true;
                DirectionX = -1f;

                newPosition = new Vector2(-StandingFirePointX, StandingFirePoint.localPosition.y);
                StandingFirePoint.localPosition = newPosition;

                newPosition = new Vector2(-SittingFirePointX, SittingFirePoint.localPosition.y);
                SittingFirePoint.localPosition = newPosition;
                return -1f;
            }
            return DirectionX;
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
            Horizontal = direction;
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

        public void AttackCheck()
        {
            //Both types of attack with single cooldown
            if (AttackTimer <= 0)
            {
                AttackTimer = PrimaryAttackCooldown;
                HitAttack = false;
                // Up + X - secondary
                if (Vertical == 1)
                {
                    ShootWeaponAxe();
                }
                else if (Vertical <= 0) // attacks from idle and sitting
                {
                    ShootWeaponKnife();
                }
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
            // have to change adressing to higher level model, e.g. "Game"
            // should be event )

            // doing nothing right now
            //GameOver.Show();
        }

        public void JumpDown()
        {
            if (PlatformInstance != null)
            {
                PlatformInstance.ComeThrough();
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
            //Hud.ChangeLivesAmount(lives);
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
            if (Input.GetButtonDown("Fire2"))
            {
                Interaction();
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

        private void ShootWeaponKnife()
        {
            var instance = ResourceManager.GetFromPool(Weapons.PlayerKnife);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);

            var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
            weaponVelocity.x *= DirectionX;
            instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

            instance.GetComponent<PlayerKnife>().Initiate(FirePoint.position, DirectionX);
        }

        private void ShootWeaponAxe()
        {
            var instance = ResourceManager.GetFromPool(Weapons.PlayerAxe);
            var dynamics = CompositionRoot.GetDynamicsContainer();
            instance.transform.SetParent(dynamics.Transform, false);
            dynamics.AddItem(instance);

            var weaponVelocity = instance.GetComponent<DamageDealer>().Velocity;
            weaponVelocity.x *= DirectionX;
            instance.GetComponent<Rigidbody2D>().velocity = weaponVelocity;

            instance.GetComponent<PlayerAxe>().Initiate(FirePoint.position, DirectionX);
        }

        private void LoadConfigData()
        {
            HorizontalSpeed = Config.HorizontalSpeed;
            CrouchSpeed = Config.CrouchSpeed;
            PushDownForce = Config.PushDownForce;
            JumpForce = Config.JumpForce;
            RollDownForce = Config.RollDownForce;

            PrimaryAttackCooldown = Config.PrimaryAttackCooldown;
            DeathShockTime = Config.DeathShockTime;

            RollDownTime = Config.RollDownTime;
            JumpDownTime = Config.JumpDownTime;
        }

    }
}