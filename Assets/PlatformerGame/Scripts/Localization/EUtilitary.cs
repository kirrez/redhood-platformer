using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public enum EUtilitary
    {
        //TitleScreen
        Play_deselected,
        Settings_deselected,
        Credits_deselected,
        Quit_deselected,

        Play_selected,
        Settings_selected,
        Credits_selected,
        Quit_selected,
        //

        Back,
        Auto,
        Manual,
        Forth,
        BackToTitle,

        //PlayScreen
        SelectYourGame_Title,
        GameNameLabel,
        DifficultyModeLabel,
        EasyMode,
        // NormalMode,
        TimePlayed,
        CreateButton,
        RenameButton,
        DeleteButton,
        PlayButton,
        NoData,
        EnterYourName_Title,
        AreYouSure_Title,
        Submit,
        Cancel,
        //

        TryAgain,
        ToMenu,

        English,
        Russian,
    }
}