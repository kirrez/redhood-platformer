using UnityEngine;

namespace Platformer
{
    public interface IUIRoot
    {
        Transform ScreenCanvas { get; }
        Transform HUDCanvas { get; }
        Transform MenuCanvas { get; }
    }
}

