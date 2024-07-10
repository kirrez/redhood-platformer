using UnityEngine;

namespace Platformer
{
    public class SharpenedAxeItem : MonoBehaviour
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
                var axeLevel = ProgressManager.GetQuest(EQuest.AxeLevel);

                if (axeLevel == 1)
                {
                    ProgressManager.SetQuest(EQuest.AxeLevel, 2);
                    ProgressManager.SetQuest(EQuest.SharpenedAxeItem, 2); // don't want to spawn it anymore

                    Player.UpdateAllWeaponLevel();

                    Game.HUD.UpdateWeaponIcons();
                    //game.FadeScreen.FadeOut(Color.white, 1.5f);
                    //AudioManager.PauseMusic(7f, true);
                    //AudioManager.PlaySound(ESounds.Collect10LifeUpgrade);

                    AudioManager.PlaySound(ESounds.Collect1QuestItems); // v 0.1
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