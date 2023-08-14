using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BlockingTrap : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> Blocks;

        [SerializeField]
        private float DurationTime = 1f;

        [SerializeField]
        private bool RearmInstantly = false;

        [SerializeField]
        private bool InvertBlocks = false;

        private bool IsTriggered;
        //private Collider2D Collider;
        private float Timer;

        private void Awake()
        {
            //Collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            IsTriggered = false;
            Timer = DurationTime;
            SetBlocks(InvertBlocks);
        }

        private void Update()
        {
            if (IsTriggered && Timer > 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    if (RearmInstantly)
                    {
                        IsTriggered = false;
                    }

                    SetBlocks(InvertBlocks);
                    Timer = DurationTime;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!IsTriggered)
                {
                    IsTriggered = true;
                    SetBlocks(!InvertBlocks);
                    Timer = DurationTime;
                }
            }
        }

        private void SetBlocks(bool state)
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].SetActive(state);
            }
        }
    }
}