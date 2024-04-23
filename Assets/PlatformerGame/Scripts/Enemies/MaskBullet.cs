using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MaskBullet : MonoBehaviour
    {
        [SerializeField]
        private Sprite Sprite;

        private IPlayer Player;
        private IAudioManager AudioManager;
        private IResourceManager ResourceManager;
        private IDynamicsContainer DynamicsContainer;

        private float DeltaX = 15f;
        private float DeltaY = 13f;

        private DamageDealer DamageDealer;

        private void Awake()
        {
            DynamicsContainer = CompositionRoot.GetDynamicsContainer();
            ResourceManager = CompositionRoot.GetResourceManager();
            AudioManager = CompositionRoot.GetAudioManager();
            DamageDealer = GetComponent<DamageDealer>();
            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = Sprite;

            DamageDealer.Destructed += OnDestruction;
        }

        private void OnEnable()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void OnDisable()
        {
            var newPosition = transform.position;
            newPosition.y -= 0.35f;
            var instance = ResourceManager.GetFromPool(GFXs.DeathFlameShort);
            DynamicsContainer.AddMain(instance);
            var effect = instance.GetComponent<DeathFlameEffect>();
            effect.Initiate(newPosition, new Vector2(1f, 1f));
            //AudioManager.PlaySound(ESounds.Slash2);

            if (AudibleDistance() == true) AudioManager.PlaySound(ESounds.Slash2);

            gameObject.SetActive(false);
        }

        private bool AudibleDistance()
        {
            if (Player == null) return false;

            var distanceX = Mathf.Abs(transform.position.x - Player.Position.x);
            var distanceY = Mathf.Abs(transform.position.y - Player.Position.y);

            if (distanceX <= DeltaX && distanceY <= DeltaY) return true;

            return false;
        }

        private void OnDestruction()
        {
            gameObject.SetActive(false);
        }
    }
}