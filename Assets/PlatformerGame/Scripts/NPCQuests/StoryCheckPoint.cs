using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class StoryCheckPoint : MonoBehaviour
    {
        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int QuestKeyIndex;

        [SerializeField]
        private int QuestValue;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var keyIndex = (EQuest)QuestKeyIndex;
            ProgressManager.SetQuest(EQuest.Location, LocationIndex);
            ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);

            // Updating specific quest, if it's value's not equal to QuestValue;
            if (QuestKeyIndex < 0)
            {
                // skip updating some quest value;
                return;
            }

            if (ProgressManager.GetQuest(keyIndex) != QuestValue)
            {
                ProgressManager.SetQuest(keyIndex, QuestValue);
            }
        }
    }
}