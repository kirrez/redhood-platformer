using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface ILocalization
    {
        string Text(ETexts name);

        string Destination(EDestinationNames name);

        string Tutorial(ETutorialTexts name);

        string Utilitary(EUtilitary name);

        string Label(ELabels name);

        void LoadLocalization(ELocalizations localization);

        ELocalizations GetLocalization();
    }
}