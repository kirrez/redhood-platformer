using UnityEngine;

namespace Platformer
{
    public class Riddle_2 : MonoBehaviour
    {
        [SerializeField]
        private GameObject Obstacles;

        [SerializeField]
        private SpriteRenderer SwitchRenderer;

        [SerializeField]
        Collider2D SwitchCollider;

        [SerializeField]
        Collider2D TrapCollider;

        [SerializeField]
        private Sprite SwitchOn;

        [SerializeField]
        private Sprite SwitchOff;

        private IPlayer Player;
        private bool TrapActivated;
        private bool SwitchTurnedOn;
        private bool InsideSwitch;

        private void OnEnable()
        {
            DeactivateTrap();
            SwitchRenderer.sprite = SwitchOff;
        }

        private void Update()
        {
            if (Player == null) return;

            if (TrapCollider.bounds.Contains(Player.Position) && !TrapActivated && !SwitchTurnedOn)
            {
                ActivateTrap();
            }

            if (SwitchCollider.bounds.Contains(Player.Position) && !InsideSwitch)
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
            if (collision.gameObject.CompareTag("Player"))
            {
                Player = collision.gameObject.GetComponent<IPlayer>();
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
            TrapActivated = false;
        }

        private void OnSwitchPulled()
        {
            Player.Interaction -= OnSwitchPulled;
            SwitchTurnedOn = true;
            SwitchRenderer.sprite = SwitchOn;
            DeactivateTrap();
        }
    }
}