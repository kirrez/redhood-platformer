using UnityEngine;

namespace Platformer
{
    public class FarmerKnifeItem : MonoBehaviour
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
                var knifeLevel = ProgressManager.GetQuest(EQuest.KnifeLevel);

                if (knifeLevel == 1)
                {
                    ProgressManager.SetQuest(EQuest.KnifeLevel, 2);
                    ProgressManager.SetQuest(EQuest.FarmerKnifeItem, 2); // don't want to spawn it anymore

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