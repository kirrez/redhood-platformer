using UnityEngine;

namespace Platformer
{
    public class Food : MonoBehaviour
    {
        // base - Food00
        [SerializeField]
        [Range(0, 19)]
        private int ItemIndex;

        private int TargetValue;
        private EQuest Item;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();

            TargetValue = (int)EQuest.Food00 + ItemIndex;
            Item = (EQuest)TargetValue;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (ProgressManager.GetQuest(Item) == 0)
                {
                    ProgressManager.SetQuest(Item, 1);
                    //increment food collected
                    ProgressManager.AddValue(EQuest.FoodCollected, 1);

                    var Game = CompositionRoot.GetGame();
                    Game.HUD.UpdateResourceAmount();
                }
                gameObject.SetActive(false);
            }
        }
    }
}