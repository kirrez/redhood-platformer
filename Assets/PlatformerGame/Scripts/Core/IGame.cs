using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.UI;

namespace Platformer
{
    public interface IGame
    {
        public DialogueModel Dialogue { get; }
    }
}