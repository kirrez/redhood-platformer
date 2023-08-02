namespace Platformer
{
    public enum EQuest
    {
        Stage,
        Location,
        SpawnPoint,

        RedKey,
        GreyKey,
        GreenKey,

        MotherPie,

        MushroomsCurrent,
        MushroomsRequired,
        BerriesCurrent,
        BerriesRequired,

        Mushroom1 = 11,
        Mushroom2 = 12,
        Mushroom3 = 13,
        Blackberry1 = 14,
        Blackberry2 = 15,
        Blackberry3 = 16,
        Blackberry4 = 17,

        SuspensionBridge,
        Drawbridge,
        WFFirstElevator = 20,
        WFSkeletonPlatform = 21,
        WFSecondElevator = 22,
        MarketElevator = 23,

        KnifeLevel, // 0-none, 1-KitchenKnife, 2-FarmerKnife, 3-HunterKnife
        AxeLevel, // 0-none, 1-CrippledAxe(no WoodBreaker), 2-SharpenedAxe, 3-SturdyAxe
        HolyWaterLevel, // 0-none, 1-WeakHolyWater(Undead Stun only), 2-StrongHolyWater(+ normal damage)

        //Player's max lives (3-8)
        MaxLives,
        MaxLivesCap,

        LifeUpgradeCost,
        FoodCollected,

        OreCollected
    }
}

