using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class KeyGrey : MonoBehaviour
    {
        private IProgressManager ProgressManager;

        private void OnEnable()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(EQuest.GreyKey) == 0)
                {
                    ProgressManager.SetQuest(EQuest.GreyKey, 1);
                }

                gameObject.SetActive(false);
            }
        }
    }
}