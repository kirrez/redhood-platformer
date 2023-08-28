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
            if (Input.GetKeyDown(Config.LeftKeyCode))
            {
                Player.MoveLeft();
            }

            if (Input.GetKeyDown(Config.RightKeyCode))
            {
                Player.MoveRight();
            }

            if (Input.GetKeyUp(Config.LeftKeyCode) && Input.GetKey(Config.RightKeyCode))
            {
                Player.MoveRight();
            }

            if (Input.GetKeyUp(Config.RightKeyCode) && Input.GetKey(Config.LeftKeyCode))
            {
                Player.MoveLeft();
            }

            if (Input.GetKeyUp(Config.LeftKeyCode) && Input.GetKey(Config.RightKeyCode) == false)
            {
                Player.Idle();
            }

            if (Input.GetKeyUp(Config.RightKeyCode) && Input.GetKey(Config.LeftKeyCode) == false)
            {
                Player.Idle();
            }

            if (Input.GetKeyDown(Config.JumpKeyCode))
            {
                Player.Jump();
            }

            if (Input.GetKeyDown(Config.FireKeyCode))
            {
                Player.Fire();
            }

            if (Input.GetKeyDown(Config.UseKeyCode))
            {
                Player.Use();
            }
        }

        public void Lock()
        {
        }

        public void Unlock()
        {
        }
    }
}