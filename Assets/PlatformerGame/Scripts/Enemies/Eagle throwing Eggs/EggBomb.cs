using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EggBomb : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Still;

        [SerializeField]
        private List<Sprite> Rotating;

        [SerializeField]
        private float AnimationDelay = 0.15f;

        [SerializeField]
        private Health Health;

        [SerializeField]
        private Collider2D DamageTrigger;

        [SerializeField]
        private DamageDealer DamageDealer;

        [SerializeField]
        private Rigidbody2D Body;

        private SpriteRenderer Renderer;
        private List<Sprite> CurrentAnimation;
        private float Timer;
        private float Delay;
        private int Index;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;
        private IPlayer Player;

        //private delegate void State();
        //State CurrentState = () => { };

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            AudioManager = CompositionRoot.GetAudioManager();
            ResourceManager = CompositionRoot.GetResourceManager();
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();

            Renderer = GetComponent<SpriteRenderer>();

            Health.Killed += OnDestruction;
            DamageDealer.Destructed += OnDestruction;
        }

        private void OnEnable()
        {
            PlayStill();
            Health.RefillHealth();
            DamageTrigger.gameObject.SetActive(true);

            //DropDown();
            //Throw();
        }

        private void FixedUpdate()
        {
            PlayAnimation();
        }

        private void PlayStill()
        {
            CurrentAnimation = Still;
            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;
        }

        private void PlayRotating()
        {
            CurrentAnimation = Rotating;
            Delay = AnimationDelay;
            Timer = Delay;
            Index = 0;
        }

        private void PlayAnimation()
        {
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                Timer = Delay;

                if (Index == CurrentAnimation.Count - 1)
                {
                    Index = 0;
                }
                else if (Index < CurrentAnimation.Count - 1)
                {
                    Index++;
                }

                Renderer.sprite = CurrentAnimation[Index];
            }
        }

        public void DropDown()
        {
            PlayStill();
            Body.gravityScale = 1f;
            Body.velocity = Vector2.zero;
            Body.AddForce(new Vector2(0f, -2f), ForceMode2D.Impulse);
        }

        public void Throw()
        {
            PlayRotating();
            DamageTrigger.gameObject.SetActive(true);

            if (Player.Position.x <= transform.position.x)
            {
                Renderer.flipX = true;
            }

            if (Player.Position.x > transform.position.x)
            {
                Renderer.flipX = false;
            }

            Body.gravityScale = 0f;
            Body.AddForce((Player.Position - transform.position).normalized * 7.5f, ForceMode2D.Impulse);
        }

        private void PlayDestructionVisual()
        {
            var newPosition = transform.position;
            newPosition.y -= 0.5f;
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameShort);
            DynamicsContainer.AddMain(instance);
            var effect = instance.GetComponent<DeathFlameEffect>();
            effect.Initiate(newPosition, new Vector2(1f, 1f));

            var amount = Random.Range(3, 7);

            for (int i = 0; i < amount; i++)
            {
                var shatter = ResourceManager.GetFromPool(GFXs.EggShell);
                DynamicsContainer.AddMain(shatter);
                shatter.GetComponent<WoodShatter>().Initiate(transform.position);
            }
        }

        private void OnDestruction()
        {
            AudioManager.PlaySound(ESounds.EggCrush);
            AudioManager.PlaySound(ESounds.Slash2);

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            PlayDestructionVisual();
            gameObject.SetActive(false);
        }
    }
}