namespace Platformer
{
    public enum EQuest
    {
        Stage,
        Location,
        SpawnPoint,

        //Mother's Pie quest
        MotherPie,
        MushroomsCollected,
        MushroomsRequired,
        BlackberriesCollected,
        BlackberriesRequired,

        //Suspension bridge quest
        SuspensionBridge,

        Drawbridge,

        KnifeLevel, // 0-none, 1-KitchenKnife, 2-FarmerKnife, 3-HunterKnife
        AxeLevel, // 0-none, 1-CrippledAxe(no WoodBreaker), 2-OldAxe(+WoodBreaker), 3-SharpenedAxe, 4-SturdyAxe
        HolyWaterLevel, // 0-none, 1-WeakHolyWater(Undead Stun only), 2-StrongHolyWater(+ normal damage)

        FarmerKnifeCost,
        HunterKnifeCost,
        SharpenedAxeCost, //in ore
        SturdyAxeCost,
        StrongHolyWaterCost,

        //Player's max lives (3-8)
        MaxLives,
        MaxLivesCap,

        LifeUpgradeCost,
        FoodCollected,
        OreCollected,

        //Collectibles, set of each type must go one after another ))
        KeyRed,
        KeyGray,
        KeyGreen,

        //Items for PlatformActivation
        WFFirstElevator,
        WFSkeletonPlatform,
        WFSecondElevator,
        MarketElevator,

        Mushroom0,
        Mushroom1,
        Mushroom2,

        Blackberry0,
        Blackberry1,
        Blackberry2,
        Blackberry3,

        Food00,
        Food01,
        Food02,
        Food03,
        Food04,
        Food05,

        Ore00,
        Ore01,
        Ore02,
        Ore03,
        Ore04,
        Ore05
    }
}

