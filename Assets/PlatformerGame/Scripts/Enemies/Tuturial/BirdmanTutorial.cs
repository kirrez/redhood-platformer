using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BirdmanTutorial : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private SpriteRenderer CheeseRenderer;

        [SerializeField]
        private SimpleAnimation Animator;

        [SerializeField]
        private Transform CheesePos1;

        [SerializeField]
        private Transform CheesePos2;

        private IDynamicsContainer DynamicsContainer;
        private IResourceManager ResourceManager;
        private IAudioManager AudioManager;

        private Transform Target;
        private TutorialCheese Cheese;

        private float Timer;
        private float BlastDelay = 1.3f;
        private float Speed = 11f;
        private Vector2 Rise = new Vector2(0f, 7f);

        delegate void State();
        State CurrentState = () => { };

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
        }

        public void Initiate(Transform target, TutorialCheese cheese)
        {
            Cheese = cheese;
            Target = target;

            
            transform.position = new Vector2(target.position.x + 10 * Random.Range(-1f, 1f), target.position.y + 15);
            if (transform.position.x <= Target.position.x)
            {
                Renderer.flipX = false;
                CheeseRenderer.gameObject.transform.localPosition = CheesePos1.localPosition;
            }
            else
            {
                Renderer.flipX = true;
                CheeseRenderer.gameObject.transform.localPosition = CheesePos2.localPosition;
            }

            CurrentState = StateAppear;
        }

        private void FixedUpdate()
        {
            CurrentState();
        }

        private void StateAppear()
        {
            Animator.enabled = true;
            Renderer.enabled = true;
            CheeseRenderer.enabled = false;

            AudioManager.PlaySound(ESounds.EagleScream);
            CurrentState = StateOncoming;
        }

        private void StateOncoming()
        {
            var direction = (Target.position - transform.position).normalized * Speed;
            Body.velocity = direction;
            if (Vector2.Distance(transform.position, Target.position) <= 0.2f)
            {
                CheeseRenderer.enabled = true;
                Cheese.TakenAway();
                Timer = BlastDelay;

                CurrentState = StateRising;
            }
        }

        private void StateRising()
        {
            Body.velocity = Rise;

            Timer -= Time.fixedDeltaTime;
            if (Timer <= 0)
            {
                Animator.enabled = false;
                Renderer.enabled = false;

                var amount = Random.Range(8, 15);
                var newPosition = Body.transform.position;
                newPosition.y += 1f;
                for (int i = 0; i < amount; i++)
                {
                    var feather = ResourceManager.GetFromPool(GFXs.FeatherParticle);
                    DynamicsContainer.AddMain(feather);
                    feather.GetComponent<FeatherParticle>().Initiate(newPosition);
                }

                AudioManager.PlaySound(ESounds.Blast8);

                var instance = ResourceManager.GetFromPool(GFXs.FireBlast);
                DynamicsContainer.AddTemporary(instance);
                instance.GetComponent<BaseGFX>().Initiate(newPosition, 1f);

                gameObject.SetActive(false);
            }
        }
    }
}