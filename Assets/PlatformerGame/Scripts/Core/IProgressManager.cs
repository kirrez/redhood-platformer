using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IProgressManager
    {
        void LoadNewGame();
        void LoadTestConfig();

        void RefillRenewables();
        int GetQuest(EQuest key);
        void SetQuest(EQuest key, int value);

        void AddValue(EQuest key, int value);
    }
}