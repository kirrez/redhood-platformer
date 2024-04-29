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

        void LoadLocalization(ELocalizations localization);
    }
}