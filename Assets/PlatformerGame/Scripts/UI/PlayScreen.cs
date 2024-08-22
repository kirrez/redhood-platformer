using System;
using UnityEngine;

namespace Platformer
{
    public class PlayScreen
    {
        private const int SlotCount = 3;

        public event Action Started = () => { };
        public event Action Canceled = () => { };

        private IPlayScreenView View;

        private IStorage Storage;
        private IProgressManager ProgressManager;

        private IPlayerState[] PlayerStates = new IPlayerState[SlotCount];

        public PlayScreen()
        {
            Storage = CompositionRoot.GetStorage();
            ProgressManager = CompositionRoot.GetProgressManager();

            var uiRoot = CompositionRoot.GetUIRoot();
            var resourceManager = CompositionRoot.GetResourceManager();

            View = resourceManager.CreatePrefab<IPlayScreenView, EViews>(EViews.PlayScreenView);
            View.SetParent(uiRoot.MenuCanvas.transform);

            View.SlotSelected += OnSlotSelected;

            View.CreateClicked += OnCreateClicked;
            View.PlayClicked += OnPlayClicked;
            View.BackClicked += OnBackClicked;

            View.DeleteClicked += OnDeleteClicked;
            View.RenameClicked += OnRenameClicked;
            View.DeleteCanceled += OnDeleteCanceled;
            View.DeleteSubmitted += OnDeleteSubmitted;
            View.RenameCanceled += OnRenameCanceled;
            View.RenameSubmitted += OnRenameSubmitted;
        }

        private void Initialize()
        {
            for (int i = 0; i < SlotCount; i++)
            {
                var playerState = Storage.Load(i);

                if (playerState == null)
                {
                    View.EmptySlot(i);
                    continue;
                }

                PlayerStates[i] = playerState;

                var name = playerState.Name;
                var dateYear = playerState.GetQuest(EQuest.DateYear);
                var dateMonth = playerState.GetQuest(EQuest.DateMonth);
                var dateday = playerState.GetQuest(EQuest.DateDay);
                var timeHours = playerState.GetQuest(EQuest.TimeHours);
                var timeMinutes = playerState.GetQuest(EQuest.TimeMinutes);

                var date = new DateTime(dateYear, dateMonth, dateday);
                var playedTime = new TimeSpan(timeHours, timeMinutes, 0);

                View.FillSlot(i);
                View.SetSlotName(i, playerState.Name);
                View.SetSlotDate(i, date);
                View.SetSlotPlayedTime(i, playedTime);
            }
        }

        public void Show()
        {
            View.Show();

            Initialize();

            if (PlayerStates[0] == null)
            {
                View.ShowCreateButton();
            }
            else
            {
                View.ShowPlayButton();
            }

            View.SelectSlot(0);
        }

        public void Hide()
        {
            View.Hide();
        }

        private void OnRenameClicked(int id)
        {
            var playerState = PlayerStates[id];

            if (playerState == null)
            {
                return;
            }

            View.ShowRenamePopup(playerState.Name);
        }

        private void OnDeleteClicked(int id)
        {
            var playerState = PlayerStates[id];

            if (playerState == null)
            {
                return;
            }

            View.ShowDeletePopup(playerState.Name);
        }

        private void OnDeleteCanceled()
        {
            View.HideDeletePopup();
        }

        private void OnRenameCanceled()
        {
            View.HideRenamePopup();
        }

        private void OnCreateClicked(int id)
        {
            var playerState = ProgressManager.CreateState(id);
            Storage.Save(id, playerState);

            View.ShowPlayButton();
            Initialize();
        }

        private void OnRenameSubmitted(int id, string newName)
        {
            var playerState = PlayerStates[id];

            if (playerState == null)
            {
                return;
            }

            playerState.Name = newName;
            Storage.Save(id, playerState);

            View.HideRenamePopup();
            Initialize();
        }

        private void OnDeleteSubmitted(int id)
        {
            PlayerStates[id] = null;
            Storage.Delete(id);

            View.HideDeletePopup();
            Initialize();
        }

        private void OnSlotSelected(int id)
        {
            var playerState = PlayerStates[id];

            if (playerState != null)
            {
                View.ShowPlayButton();
            }
            else
            {
                View.ShowCreateButton();
            }
        }

        private void OnBackClicked()
        {
            Canceled?.Invoke();
        }

        private void OnPlayClicked(int slotID)
        {
            var selectedPlayerState = PlayerStates[slotID];

            ProgressManager.SetState(selectedPlayerState);

            Started?.Invoke();
        }
    }
}