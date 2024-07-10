using UnityEngine;

namespace Platformer
{
    public interface IFadeScreen
    {
        void FadeIn(Color color, float time);
        void FadeOut(Color color, float time);
        void DelayAfter(float delay);
        void DelayBefore(Color color, float time);
        void Show();
        void Hide();
    }
}