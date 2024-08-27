using UnityEngine;

namespace Platformer
{
    public interface IView
    {
        void Show();
        void Hide();
        void SetParent(Transform parent);
    }
}