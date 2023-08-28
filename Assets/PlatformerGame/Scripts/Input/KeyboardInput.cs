using UnityEngine;

namespace Platformer
{
    public class KeyboardInput : MonoBehaviour, IPlayerInput
    {
        private IPlayer Player;
        private KeyboardConfig Config = new KeyboardConfig();

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
        }

        private void Update()
        {

        }

        public void Lock()
        {
        }

        public void Unlock()
        {
        }

        public void GetInput()
        {
            var vertical = Input.GetAxisRaw("Vertical");
            var horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(Config.JumpKeyCode))
            {

            }

            if (Input.GetKeyUp(Config.JumpKeyCode))
            {

            }

            if (Input.GetKeyDown(Config.FireKeyCode))
            {
            }

            if (Input.GetKeyDown(Config.UseKeyCode))
            {
                
            }
        }
    }
}