using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer
{
    public class HUDView : BaseScreenView
    {
        [SerializeField]
        private List<GameObject> LivesBack;

        [SerializeField]
        private List<GameObject> LivesFront;

        [SerializeField]
        private Image KnifeImage;

        [SerializeField]
        private Image AxeImage;

        [SerializeField]
        private Image HolyWaterImage;

        [SerializeField]
        private List<Sprite> KnifeSprites;

        [SerializeField]
        private List<Sprite> AxeSprites;

        [SerializeField]
        private List<Sprite> HolyWaterSprites;

        [SerializeField]
        private Text FoodAmount;

        [SerializeField]
        private Text OreAmount;

        private IProgressManager ProgressManager;

        private void Awake()
        {
            ProgressManager = CompositionRoot.GetProgressManager();
        }

        public void UpdateWeaponIcons()
        {
            var knife = ProgressManager.GetQuest(EQuest.KnifeLevel);
            var axe = ProgressManager.GetQuest(EQuest.AxeLevel);
            var holyWater = ProgressManager.GetQuest(EQuest.HolyWaterLevel);

            KnifeImage.sprite = KnifeSprites[knife];
            AxeImage.sprite = AxeSprites[axe];
            HolyWaterImage.sprite = HolyWaterSprites[holyWater];
        }

        public void UpdateResourceAmount()
        {
            var ore = ProgressManager.GetQuest(EQuest.OreCollected);
            var food = ProgressManager.GetQuest(EQuest.FoodCollected);

            OreAmount.text = ore.ToString("00");
            FoodAmount.text = food.ToString("00");
        }

        public void SetMaxLives(int amount)
        {
            var progressManager = CompositionRoot.GetProgressManager();
            int livesCap = progressManager.GetQuest(EQuest.MaxLivesCap);
            for (int i = 0; i < livesCap; i++)
            {
                if (i <= (amount-1))
                {
                    LivesBack[i].gameObject.SetActive(true);
                }

                if (i > (amount - 1))
                {
                    LivesBack[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetCurrentLives(int amount)
        {
            var progressManager = CompositionRoot.GetProgressManager();
            int livesCap = progressManager.GetQuest(EQuest.MaxLivesCap);
            for (int i = 0; i < livesCap; i++)
            {
                if (i <= (amount - 1))
                {
                    LivesFront[i].gameObject.SetActive(true);
                }

                if (i > (amount - 1))
                {
                    LivesFront[i].gameObject.SetActive(false);
                }
            }
        }
    }
}