using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class GhostWall : CursedObstacle
    {
        [SerializeField]
        private Rigidbody2D Body;

        [SerializeField]
        private Material BlendMaterial;

        [SerializeField]
        private Material NormalMaterial;

        [SerializeField]
        private SpriteRenderer Renderer;

        [SerializeField]
        private List<Sprite> Sprites;

        [SerializeField]
        private Color BlinkIn;

        [SerializeField]
        private Color BlinkOut;

        [SerializeField]
        private float BlinkDelay = 1f;

        [SerializeField]
        private float DispelDelay = 1f;

        [SerializeField]
        private float RegenerateDelay = 4f;

        [SerializeField]
        private float RegenerateAppearDelay = 1f;

        [SerializeField]
        private float AppearingDelay = 0.15f;

        [SerializeField]
        private bool IsActive;

        private Color BlinkColor;
        private Color DispelColor;
        private float Timer;
        private float AppearTimer;
        private bool IsOpaque;
        private string TextureName = "_BlendColor";

        protected delegate void State();
        protected State CurrentState = () => { };

        private void Awake()
        {
            Dispel = OnDispel;
        }

        private void OnEnable()
        {
            CurrentState = StateSetup;
        }

        private void Update()
        {
            CurrentState();
        }

        public void Activate()
        {
            IsActive = true;
            CurrentState = StateSetup;
        }

        private void OnDispel()
        {
            Timer = DispelDelay;
            Renderer.sprite = Sprites[1];
            Renderer.material = NormalMaterial;
            DispelColor = Color.white;

            CurrentState = StateDispelProgress;
        }

        private void StateSetup()
        {
            if (IsActive == true)
            {
                Body.gameObject.SetActive(true);
                Renderer.gameObject.SetActive(true);
                Renderer.sprite = Sprites[0];
                CurrentState = StateBlink;
            }

            if (IsActive == false)
            {
                Body.gameObject.SetActive(false);
                Renderer.gameObject.SetActive(false);
                CurrentState = () => { };
            }
        }

        private void StateBlink()
        {
            BlinkColor = Color.Lerp(BlinkIn, BlinkOut, Mathf.PingPong(Time.time, BlinkDelay));
            Renderer.material.SetColor(TextureName, BlinkColor);
        }

        private void StateDispelProgress()
        {
            Timer -= Time.deltaTime;

            DispelColor.a = Timer / DispelDelay;

            Renderer.color = DispelColor;

            if (Timer <= 0)
            {
                Body.gameObject.SetActive(false);
                Timer = RegenerateDelay;
                CurrentState = StateInvisible;
            }
        }

        private void StateInvisible()
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = RegenerateAppearDelay;
                AppearTimer = AppearingDelay;
                IsOpaque = true;

                CurrentState = StateAppearing;
            }
        }

        private void StateAppearing()
        {
            Timer -= Time.deltaTime;
            AppearTimer -= Time.deltaTime;

            if (AppearTimer <= 0)
            {
                IsOpaque = !IsOpaque;
                AppearTimer = AppearingDelay;

                if (IsOpaque == true)
                {
                    DispelColor.a = 0.15f;
                    Renderer.color = DispelColor;
                }

                if (IsOpaque == false)
                {
                    DispelColor.a = 1f;
                    Renderer.color = DispelColor;
                }
            }

            if (Timer <= 0)
            {
                Renderer.material = BlendMaterial;
                Body.gameObject.SetActive(true);
                Renderer.sprite = Sprites[0];

                CurrentState = StateBlink;
            }
        }
    }
}