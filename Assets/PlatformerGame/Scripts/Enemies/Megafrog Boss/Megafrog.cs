using System.Collections;
using System.Collections.Generic;
using Platformer.MegafrogBoss;
using UnityEngine;

namespace Platformer
{
    public class Megafrog : MonoBehaviour
    {
        //Stage Positions
        public List<Transform> BottomRow;

        public List<Transform> CenterRow;

        public List<Transform> TopRow;

        public List<Transform> Marks;

        public Transform WaterLevel;

        public Transform ToadRainLevel;

        [Space]
        [Header("ANIMATIONS")]

        //Animations
        [SerializeField]
        public Sprite Idle;

        [SerializeField]
        public Sprite JumpRise;

        [SerializeField]
        public Sprite JumpFall;

        [SerializeField]
        public Sprite Attack;

        [Header("COMPONENTS")]

        [SerializeField]
        public Rigidbody2D Body;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private PolygonCollider2D Damager;

        public Transform HitBoxTransform;

        public Transform FirePoint;

        public HitBox HitBox;

        [SerializeField]
        private Health Health;
        private int HitPoints;


        private Leap Leap;
        private Defeat Defeat;
        private ToadRain ToadRain;
        private SpikeVomit SpikeVomit;
        private MultipleShots MultipleShots;
        private Disappearance Disappearance;
        private Appearance Appearance;

        private Collider2D Collider;
        private Vector3 LastPosition;

        public MegafrogFight MegafrogFight { get; set; }
        public float DirectionX { get; set; } = 1f;
        public float DeltaY { get; set; }

        private float FirePointX;
        private float HitBoxX;
        public int Rage { get; set; }
        public int Phase { get; set; }

        public delegate void State();
        State CurrentState = () => { };

        public IPlayer Player;

        public void SetState(State state)
        {
            CurrentState = state;
        }

        public void SetAnimation(Sprite sprite)
        {
            Renderer.sprite = sprite;
        }

        public void SetDirectionX(float direction)
        {
            DirectionX = direction;
        }

        public void UpdateLastBodyPosition()
        {
            LastPosition.y = Body.transform.position.y;
        }

        public void Initiate(MegafrogFight fight, int maxLives)
        {
            MegafrogFight = fight;

            Rage = 0;
            Phase = 0;
            Health.SetMaxLives(maxLives);

            HitPoints = Health.GetHitPoints;
            Health.HealthChanged = null;
            Health.HealthChanged += OnHealthChanged;

            Health.DamageCooldownExpired = null;
            Health.DamageCooldownExpired += OnDamageCooldownExpired;

            SetState(Appearance.StartFirstTime);
        }

        private void OnHealthChanged()
        {
            var game = CompositionRoot.GetGame();

            HitPoints = Health.GetHitPoints;
            //Update UI hitPoints
            game.HUD.SetEnemyCurrentHealth(HitPoints);

            //Actions if "hitpoints > 0"
            if (HitPoints > 0)
            {

            }
            //Actions if "hitpoints = 0"

            if (HitPoints == 0)
            {
                SetState(Defeat.Start);
            }
        }

        private void OnDamageCooldownExpired()
        {
            //Change "damaged" animation back to normal
        }

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            //ResourceManager = CompositionRoot.GetResourceManager();

            Collider = Body.GetComponent<Collider2D>();

            FirePointX = FirePoint.localPosition.x;
            HitBoxX = HitBoxTransform.localPosition.x;
            Hide();

            Leap = new Leap(this);
            Defeat = new Defeat(this);
            ToadRain = new ToadRain(this);
            SpikeVomit = new SpikeVomit(this);
            MultipleShots = new MultipleShots(this);
            Disappearance = new Disappearance(this);
            Appearance = new Appearance(this);
        }

        private void FixedUpdate()
        {
            DeltaY = Body.transform.position.y - LastPosition.y;
            UpdateLastBodyPosition();

            CurrentState();
        }

        public void DecidePhase()
        {
            //Appearance
            if (Phase == 0)
            {
                var chance = UnityEngine.Random.Range(0, 5);
                int[] choices = { 0, 0, 0, 1, 1 };
                var choice = choices[chance];

                if (choice == 0) SetState(Appearance.StartFromAbove);
                if (choice == 1) SetState(Appearance.StartFromBeneath);
                return;
            }

            //Attacks and Leaps
            if (Phase > 0 && Phase < 3)
            {
                if (Rage == 5)
                {
                    SetState(ToadRain.Start);
                    return;
                }

                var chance = UnityEngine.Random.Range(0, 12);
                int[] choices = { 0, 0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3 };
                var choice = choices[chance];

                if (choice == 0) SetState(Leap.StartLeap);
                if (choice == 1) SetState(Leap.StartShot);
                if (choice == 2) SetState(MultipleShots.Start);
                if (choice == 3) SetState(SpikeVomit.Start);

                return;
            }
            
            //Disappearances
            if (Phase == 3)
            {
                Rage++;
                SetState(Disappearance.Start);
            }
        }

        public bool IsGrounded(LayerMask mask)
        {
            var origin = new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
            var boxSize = new Vector2(Collider.bounds.size.x, 0.1f);

            float distance = 0.1f; // Magic number, empirical

            RaycastHit2D GroundHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, distance, mask);

            return GroundHit.collider != null;
        }

        public void SetMask(string mask)
        {
            Body.gameObject.layer = LayerMask.NameToLayer(mask);
        }

        public void FacePlayer()
        {
            var frogX = Body.transform.position.x;
            var playerX = Player.Position.x;

            if (frogX <= playerX) DirectionX = 1f;
            if (frogX > playerX) DirectionX = -1f;
            ChangeDirection();
        }

        public void ChangeDirection()
        {
            Vector2 newPosition;
            if (DirectionX == 1f)
            {
                Renderer.flipX = false;
                newPosition = new Vector2(FirePointX, FirePoint.localPosition.y);
                FirePoint.localPosition = newPosition;

                newPosition = new Vector2(HitBoxX, HitBoxTransform.localPosition.y);
                HitBoxTransform.localPosition = newPosition;
            }
            if (DirectionX == -1f)
            {
                Renderer.flipX = true;
                newPosition = new Vector2(-FirePointX, FirePoint.localPosition.y);
                FirePoint.localPosition = newPosition;

                newPosition = new Vector2(-HitBoxX, HitBoxTransform.localPosition.y);
                HitBoxTransform.localPosition = newPosition;
            }
        }

        public void Show()
        {
            Body.gameObject.SetActive(true);
        }

        public void Hide()
        {
            Body.gameObject.SetActive(false);
        }

        public void Freeze()
        {
            Body.isKinematic = true;
        }

        public void Unfreeze()
        {
            Body.isKinematic = false;
        }

        public void DisableBodyDamage()
        {
            Damager.enabled = false;
        }
    }
}