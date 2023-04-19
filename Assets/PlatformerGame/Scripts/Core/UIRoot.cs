
using UnityEngine;

namespace Platformer
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        public Transform OverlayCanvasLink;
        public Transform MainCanvasLink;
        public Transform MenuCanvasLink;

        public Transform OverlayCanvas => OverlayCanvasLink;
        public Transform MainCanvas => MainCanvasLink;

        public Transform MenuCanvas => MenuCanvasLink;
    }
}