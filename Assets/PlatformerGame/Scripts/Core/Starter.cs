using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Starter : MonoBehaviour
    {
        private void Awake()
        {
            var game = CompositionRoot.GetGame();
        }
    }
}