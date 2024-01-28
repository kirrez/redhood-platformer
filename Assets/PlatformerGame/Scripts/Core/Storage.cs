using System;
using UnityEngine;

namespace Platformer
{
    public class Storage : IStorage
    {
        private const int StorageVersion = 1;
        private const string StorageVersionKey = "StorageVersion";
        private const string PlayerStateKey = "PlayerState_{0}";
        private const string QuestKey = "PlayerState_{0}_{1}";

        public bool IsPlayerStateExists(int ID)
        {
            if (PlayerPrefs.HasKey(StorageVersionKey))
            {
                var version = PlayerPrefs.GetInt(StorageVersionKey);

                if (StorageVersion != version)
                {
                    return false;
                }
            }

            var playerStateKey = string.Format(PlayerStateKey, ID);
            var isExists = PlayerPrefs.HasKey(playerStateKey);

            if (isExists == false)
            {
                return false;
            }

            return true;
        }

        public IPlayerState LoadPlayerState(int ID)
        {
            if (PlayerPrefs.HasKey(StorageVersionKey))
            {
                var version = PlayerPrefs.GetInt(StorageVersionKey);

                if (StorageVersion != version)
                {
                    return null;
                }
            }

            var playerStateKey = string.Format(PlayerStateKey, ID);
            var isExists = PlayerPrefs.HasKey(playerStateKey);

            if (isExists == false)
            {
                return null;
            }

            var result = new PlayerState();
            var values = Enum.GetValues(typeof(EQuest));

            foreach (var value in values)
            {
                var questType = (EQuest)value;
                var questKey = string.Format(QuestKey, ID, questType);
                var questValue = PlayerPrefs.GetInt(questKey);

                result.SetQuest(questType, questValue);
            }

            return result;
        }

        public void Save(IPlayerState playerState)
        {
            if (PlayerPrefs.HasKey(StorageVersionKey))
            {
                var version = PlayerPrefs.GetInt(StorageVersionKey);

                if (StorageVersion != version)
                {
                    PlayerPrefs.DeleteAll();
                }
            }

            PlayerPrefs.SetInt(StorageVersionKey, StorageVersion);

            var values = Enum.GetValues(typeof(EQuest));

            foreach (var value in values)
            {
                var questType = (EQuest)value;
                var questValue = playerState.GetQuest(questType);
                var questKey = string.Format(QuestKey, playerState.ID, questType);

                PlayerPrefs.SetInt(questKey, questValue);
            }
        }
    }
}