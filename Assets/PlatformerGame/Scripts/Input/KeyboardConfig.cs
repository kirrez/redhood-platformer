using UnityEngine;

namespace Platformer
{
    public class KeyboardConfig
    {
        public KeyCode UpKeyCode;
        public KeyCode DownKeyCode;
        public KeyCode LeftKeyCode;
        public KeyCode RightKeyCode;

        public KeyCode JumpKeyCode;
        public KeyCode FireKeyCode;
        public KeyCode UseKeyCode;

        public KeyboardConfig()
        {
            UpKeyCode = KeyCode.UpArrow;
            DownKeyCode = KeyCode.DownArrow;
            LeftKeyCode = KeyCode.LeftArrow;
            RightKeyCode = KeyCode.RightArrow;

            JumpKeyCode = KeyCode.Z;
            FireKeyCode = KeyCode.X;
            UseKeyCode = KeyCode.C;
        }
    }
}