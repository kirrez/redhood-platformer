using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IGame
    {
        public static HUDModel HUD { get; }
        public static DialogueModel Dialogue { get; }
        public static FadeScreenModel FadeScreen { get; }
        public static GameOverModel GameOver { get; }

        public void GameOverMenu();
    }
}