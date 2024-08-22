using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IMessageCanvas
    {
        void SetPosition(Vector3 position);
        void SetMessage(string message);

        void SetBlinking(bool blinking, float period);
        void StopBlinking();
        void Show();
        void Hide();
    }
}