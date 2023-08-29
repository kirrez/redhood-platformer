using UnityEngine;

namespace Platformer
{
    public abstract class BaseScreenView : MonoBehaviour, IView
    {
        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }
    }
}