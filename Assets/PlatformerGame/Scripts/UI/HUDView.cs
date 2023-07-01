using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HUDView : BaseScreenView
    {
        [SerializeField]
        private List<GameObject> LivesBack;

        [SerializeField]
        private List<GameObject> LivesFront;

        public void SetMaxLives(int amount)
        {
            var progressManager = CompositionRoot.GetProgressManager();
            int livesCap = progressManager.GetQuest(EQuest.MaxLivesCap);
            for (int i = 0; i < livesCap; i++)
            {
                if (i <= (amount-1))
                {
                    LivesBack[i].gameObject.SetActive(true);
                }

                if (i > (amount - 1))
                {
                    LivesBack[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetCurrentLives(int amount)
        {
            var progressManager = CompositionRoot.GetProgressManager();
            int livesCap = progressManager.GetQuest(EQuest.MaxLivesCap);
            for (int i = 0; i < livesCap; i++)
            {
                if (i <= (amount - 1))
                {
                    LivesFront[i].gameObject.SetActive(true);
                }

                if (i > (amount - 1))
                {
                    LivesFront[i].gameObject.SetActive(false);
                }
            }
        }
    }
}