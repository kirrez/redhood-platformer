using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IGame
    {
        public HUDModel HUD { get; }
        public DialogueModel Dialogue { get; }
        public FadeScreenModel FadeScreen { get; }
        public GameOverModel GameOver { get; }

        public void GameOverMenu();
    }
}