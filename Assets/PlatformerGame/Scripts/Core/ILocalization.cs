using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface ILocalization
    {
        string Text(ETexts text);
        void LoadLocalization(ELocalizations localization);
    }
}