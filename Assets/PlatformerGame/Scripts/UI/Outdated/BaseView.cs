using UnityEngine;

namespace Platformer
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }
    }
}