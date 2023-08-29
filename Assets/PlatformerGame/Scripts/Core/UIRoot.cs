using UnityEngine;

namespace Platformer
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        public Transform ScreenCanvasLink;
        public Transform HUDCanvasLink;
        public Transform MenuCanvasLink;
        public Transform InputCanvasLink;

        public Transform ScreenCanvas => ScreenCanvasLink;
        public Transform HUDCanvas => HUDCanvasLink;
        public Transform MenuCanvas => MenuCanvasLink;
        public Transform InputCanvas => InputCanvasLink;
    }
}