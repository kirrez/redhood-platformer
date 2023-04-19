using UnityEngine;

namespace Platformer
{
    public interface IUIRoot
    {
        Transform OverlayCanvas { get; }
        Transform MainCanvas { get; }
        Transform MenuCanvas { get; }
    }
}

