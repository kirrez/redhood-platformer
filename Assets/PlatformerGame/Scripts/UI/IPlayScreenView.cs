using System;

namespace Platformer
{
    public interface IPlayScreenView : IView
    {
        event Action<int> SlotSelected;

        event Action<int> PlayClicked;
        event Action<int> CreateClicked;
        event Action<int> RenameClicked;
        event Action<int> DeleteClicked;

        event Action DeleteCanceled;
        event Action RenameCanceled;
        event Action<int> DeleteSubmitted;
        event Action<int, string> RenameSubmitted;

        event Action BackClicked;

        void SelectSlot(int index);

        void FillSlot(int index);
        void EmptySlot(int index);

        void SetSlotName(int index, string name);
        void SetSlotDateTime(int index, DateTime date, TimeSpan time);
        void SetSlotPlayedTime(int index, TimeSpan time);
        void SetSlotDifficulty(int index, int difficultyMode);

        void ShowPlayButton();
        void ShowCreateButton();

        void ShowRenamePopup(string name);
        void HideRenamePopup();

        void ShowDeletePopup(string name);
        void HideDeletePopup();
    }
}