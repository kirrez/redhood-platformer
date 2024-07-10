using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class StageTransition : MonoBehaviour
    {
        [SerializeField]
        private EStages Stage;

        [SerializeField]
        private int LocationIndex;

        [SerializeField]
        private int SpawnPointIndex;

        [SerializeField]
        private int ConfinerIndex;

        //private IPlayer Player;

        [SerializeField]
        private float TransitionTime;

        private IProgressManager ProgressManager;
        private IDynamicsContainer Dynamics;
        private INavigation Navigation;
        private IStorage Storage;

        private float Timer;
        private bool Inside;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
            Dynamics = CompositionRoot.GetDynamicsContainer();
            Navigation = CompositionRoot.GetNavigation();
            Storage = CompositionRoot.GetStorage();
        }

        private void Update()
        {
            if (Inside)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    Dynamics.DeactivateMain();
                    Dynamics.DeactivateEnemies();
                    Dynamics.DeactivateTemporary(); //temporary solution )))


                    Navigation.LoadStage(Stage);
                    Navigation.SetLocation(LocationIndex, SpawnPointIndex, ConfinerIndex);

                    var game = CompositionRoot.GetGame();
                    Game.FadeScreen.DelayBefore(Color.black, 1f);
                    Game.FadeScreen.FadeOut(Color.black, 1f);

                    // saving location data
                    ProgressManager.SetQuest(EQuest.Stage, (int)Stage);
                    ProgressManager.SetQuest(EQuest.Location, LocationIndex);
                    ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
                    ProgressManager.SetQuest(EQuest.Confiner, ConfinerIndex);

                    ProgressManager.AddPlayedTime();
                    Storage.Save(ProgressManager.PlayerState);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //Player = collision.gameObject.GetComponent<IPlayer>();
                Inside = true;
                Timer = TransitionTime;
                var game = CompositionRoot.GetGame();
                Game.FadeScreen.FadeIn(Color.black, 1f);
                Game.FadeScreen.DelayAfter(1f);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Inside = false;
                Timer = 0f;
                var game = CompositionRoot.GetGame();
                Game.FadeScreen.FadeOut(Color.black, 1f);
            }
        }
    }
}