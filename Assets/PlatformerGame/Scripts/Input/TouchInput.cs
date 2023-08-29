using UnityEngine;

namespace Platformer
{
    public class TouchInput : MonoBehaviour, IUserInput
    {
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string FireButton = "Fire";
        private const string UseButton = "Use";

        private IPlayer Player;
        private ITouchInputView View;

        private void Awake()
        {
            Player = CompositionRoot.GetPlayer();
            var uiRoot = CompositionRoot.GetUIRoot();
            var resourceManager = CompositionRoot.GetResourceManager();

            View = resourceManager.CreatePrefab<ITouchInputView, EScreens>(EScreens.InputView);
            View.SetParent(uiRoot.MenuCanvas.transform);
        }

        private void Update()
        {
            if (SimpleInput.GetAxis(HorizontalAxis) < 0f)
            {
                Player.MoveLeft();
            }

            if (SimpleInput.GetAxis(HorizontalAxis) > 0f)
            {
                Player.MoveRight();
            }

            if (SimpleInput.GetAxis(HorizontalAxis) == 0f)
            {
                Player.Idle();
            }

            if (SimpleInput.GetAxis(VerticalAxis) < 0f)
            {
                Player.Crouch();
            }

            if (SimpleInput.GetAxis(VerticalAxis) == 0f)
            {
                Player.Stand();
            }

            if (SimpleInput.GetAxis(VerticalAxis) > 0f)
            {
                Player.Jump();
            }

            if (SimpleInput.GetButton(FireButton))
            {
                Player.Fire();
            }

            if (SimpleInput.GetButton(UseButton))
            {
                Player.Use();
            }
        }

        public void Enable()
        {
            View.Enable();
        }

        public void Disable()
        {
            View.Disable();
        }
    }
}