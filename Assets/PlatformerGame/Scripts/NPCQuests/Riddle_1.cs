using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Riddle_1 : MonoBehaviour
    {
        [SerializeField]
        private GameObject Obstacles;

        [SerializeField]
        private SpriteRenderer SwitchRenderer;

        [SerializeField]
        Collider2D SwitchCollider;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private Sprite SwitchOff;

        private IPlayer Player;
        private bool TrapActivated;
        private bool InsideSwitch;

        private void OnEnable()
        {
            DeactivateTrap();
        }

        private void Update()
        {
            if (Player == null) return;

            if (SwitchCollider.bounds.Contains(Player.Position) && !InsideSwitch && TrapActivated)
            {
                Player.Interaction += OnSwitchPulled;
                InsideSwitch = true;
            }

            if (!SwitchCollider.bounds.Contains(Player.Position) && InsideSwitch)
            {
                Player.Interaction -= OnSwitchPulled;
                InsideSwitch = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !TrapActivated)
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
                ActivateTrap();
            }
        }

        private void ActivateTrap()
        {
            Obstacles.SetActive(true);
            SwitchRenderer.sprite = SwitchOff;
            TrapActivated = true;
        }

        private void DeactivateTrap()
        {
            Obstacles.SetActive(false);
            SwitchRenderer.sprite = SwitchOn;
            TrapActivated = false;
        }

        private void OnSwitchPulled()
        {
            Player.Interaction -= OnSwitchPulled;
            SwitchRenderer.sprite = SwitchOn;
            DeactivateTrap();
        }
    }
}