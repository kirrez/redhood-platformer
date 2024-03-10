using UnityEngine;

namespace Platformer
{
    public class WeakHolyWaterItem : MonoBehaviour
    {
        private IProgressManager ProgressManager;
        private IAudioManager AudioManager;
        private Rigidbody2D Rigidbody;
        private IPlayer Player;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            AudioManager = CompositionRoot.GetAudioManager();
            Rigidbody = GetComponent<Rigidbody2D>();
            Player = CompositionRoot.GetPlayer();

            PhysicsOn(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var waterLevel = ProgressManager.GetQuest(EQuest.HolyWaterLevel);

                var game = CompositionRoot.GetGame();

                if (waterLevel == 0)
                {
                    ProgressManager.SetQuest(EQuest.HolyWaterLevel, 1);
                    ProgressManager.SetQuest(EQuest.WeakHolyWaterItem, 2); // don't want to spawn it anymore

                    Player.UpdateAllWeaponLevel();

                    game.HUD.UpdateWeaponIcons();
                    game.FadeScreen.FadeOut(Color.white, 1.5f);
                    AudioManager.PauseMusic(7f, true);
                    AudioManager.PlaySound(ESounds.Collect10LifeUpgrade);
                }

                gameObject.SetActive(false);
            }
        }

        public void PhysicsOn(bool physics)
        {
            Rigidbody.isKinematic = !physics;
        }
    }
}