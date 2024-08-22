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
        private const string KeyName = "Name";

        public bool IsExists(int ID)
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

        public IPlayerState Load(int id)
        {
            string key;

            if (PlayerPrefs.HasKey(StorageVersionKey))
            {
                var version = PlayerPrefs.GetInt(StorageVersionKey);

                if (StorageVersion != version)
                {
                    return null;
                }
            }

            var playerStateKey = string.Format(PlayerStateKey, id);
            var isExists = PlayerPrefs.HasKey(playerStateKey);

            if (isExists == false)
            {
                return null;
            }

            var result = new PlayerState();

            // params not from EQuest
            key = string.Format(PlayerStateKey, id) + "_" + KeyName;
            result.Name = PlayerPrefs.GetString(key);

            //

            var values = Enum.GetValues(typeof(EQuest));

            foreach (var value in values)
            {
                var questType = (EQuest)value;
                var questKey = string.Format(QuestKey, id, questType);
                var questValue = PlayerPrefs.GetInt(questKey);

                result.SetQuest(questType, questValue);
            }

            return result;
        }

        public void Save(int id, IPlayerState playerState)
        {
            string key;

            if (PlayerPrefs.HasKey(StorageVersionKey))
            {
                var version = PlayerPrefs.GetInt(StorageVersionKey);

                if (StorageVersion != version)
                {
                    PlayerPrefs.DeleteAll();
                }
            }

            PlayerPrefs.SetInt(StorageVersionKey, StorageVersion);

            var playerStateKey = string.Format(PlayerStateKey, id);
            PlayerPrefs.SetInt(playerStateKey, 0);

            key = string.Format(PlayerStateKey, id) + "_" + KeyName;
            PlayerPrefs.SetString(key, playerState.Name);

            playerState.UpdateTimeAndDate();

            var values = Enum.GetValues(typeof(EQuest));

            foreach (var value in values)
            {
                var questType = (EQuest)value;
                var questValue = playerState.GetQuest(questType);
                var questKey = string.Format(QuestKey, id, questType);

                PlayerPrefs.SetInt(questKey, questValue);
            }
        }

        public void Delete(int id)
        {
            string key;

            if (IsExists(id) == true)
            {
                key = string.Format(PlayerStateKey, id);
                PlayerPrefs.DeleteKey(key);

                key = string.Format(PlayerStateKey, id) + "_" + KeyName;
                PlayerPrefs.DeleteKey(key);

                var values = Enum.GetValues(typeof(EQuest));
                foreach (var value in values)
                {
                    var questType = (EQuest)value;
                    //var questValue = playerState.GetQuest(questType);
                    var questKey = string.Format(QuestKey, id, questType);

                    PlayerPrefs.DeleteKey(questKey);
                }
            }
        }
    }
}