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
                    Dynamics.DeactivateAll();
                    CompositionRoot.LoadStage(Stage);
                    CompositionRoot.SetLocation(LocationIndex, SpawnPointIndex);

                    // saving location data
                    ProgressManager.SetQuest(EQuest.Stage, (int)Stage);
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
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Inside = false;
                Timer = 0f;
            }
        }
    }
}