using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TutorialCheese : BaseGFX
    {
        protected DamageDealer DamageDealer;
        private float Timer;
        private float Delay = 1.5f;
        private bool BirdSpawned;

        protected override void Awake()
        {
            base.Awake();
            DamageDealer = GetComponent<DamageDealer>();

            DamageDealer.Destructed -= OnDestructed;
            DamageDealer.Destructed += OnDestructed;
        }

        private void OnEnable()
        {
            Timer = Delay;
            BirdSpawned = false;
        }

        private void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;
            if (Timer <= 0 && !BirdSpawned)
            {
                BirdSpawned = true;
                SpawnBird();
            }
        }

        public void TakenAway()
        {
            OnDestructed();
        }

        private void SpawnBird()
        {
            var instance = ResourceManager.GetFromPool(Enemies.BirdmanTutorial);
            DynamicsContainer.AddEnemy(instance);

            instance.GetComponent<BirdmanTutorial>().Initiate(transform, this);
        }

        protected virtual void OnDestructed()
        {
            gameObject.SetActive(false);
        }

        protected override void OnDisappear()
        {
            base.OnDisappear();
        }
    }
}