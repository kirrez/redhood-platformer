using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WeaponsResetter : BaseQuest
    {
        protected override void RequirementsCheck()
        {
            if (ProgressManager.GetQuest(EQuest.StartGameWeaponsReset) > 0) return;

            if (Trigger.bounds.Contains(Player.Position) && !Inside)
            {
                Inside = true;
                Player.Interaction += OnInteraction;
                // instant beginning
                OnInteraction();
            }

            if (!Trigger.bounds.Contains(Player.Position) && Inside)
            {
                Inside = false;
                Player.Interaction -= OnInteraction;
            }
        }

        private void OnInteraction()
        {
            ProgressManager.SetQuest(EQuest.StartGameWeaponsReset, 1);

            ProgressManager.SetQuest(EQuest.KnifeLevel, 1);      // 1
            ProgressManager.SetQuest(EQuest.AxeLevel, 0);        // 0
            ProgressManager.SetQuest(EQuest.HolyWaterLevel, 0);  // 0

            ProgressManager.SetQuest(EQuest.MaxLives, 3); // 3
            Player.UpdateMaxLives();

            Player.UpdateAllWeaponLevel();
            Game.HUD.UpdateWeaponIcons();
            Player.Interaction -= OnInteraction;
        }
    }
}