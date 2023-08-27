using UnityEngine;
using System;

namespace Platformer
{
    public interface IPlayer
    {
        event Action Interaction;

        Transform Transform { get; }
        Vector3 Position { get; set; }

        float DirectionCheck(); //Input

        void Initiate(IGame game);
        void UpdateMaxLives();
        void Revive();

        void HoldByInteraction();
        void ReleasedByInteraction();
        void Stun(float time);
    }
}