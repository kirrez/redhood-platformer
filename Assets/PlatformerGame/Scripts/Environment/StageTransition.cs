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

        private IDynamicsContainer Dynamics;
        private IProgressManager ProgressManager;
        private float Timer;
        private bool Inside;

        private void Awake()
        {
            Dynamics = CompositionRoot.GetDynamicsContainer();
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void Update()
        {
            if (Inside)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    var navigation = CompositionRoot.GetNavigation();
                    Dynamics.DeactivateAll();
                    navigation.LoadStage(Stage);
                    navigation.SetLocation(LocationIndex, SpawnPointIndex, ConfinerIndex);

                    var game = CompositionRoot.GetGame();
                    game.FadeScreen.DelayBefore(Color.black, 1f);
                    game.FadeScreen.FadeOut(Color.black, 1f);

                    // saving location data
                    ProgressManager.SetQuest(EQuest.Stage, (int)Stage);
                    ProgressManager.SetQuest(EQuest.Location, LocationIndex);
                    ProgressManager.SetQuest(EQuest.SpawnPoint, SpawnPointIndex);
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
                game.FadeScreen.FadeIn(Color.black, 1f);
                game.FadeScreen.DelayAfter(1f);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Inside = false;
                Timer = 0f;
                var game = CompositionRoot.GetGame();
                game.FadeScreen.FadeOut(Color.black, 1f);
            }
        }
    }
}