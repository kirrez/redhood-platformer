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
        UpgradeHealth, // repeating quest, giving health upgrades infinitely
        Blacksmith, // giving GREY key for 3 ore
        PlatformSuspended, //quest in mountains

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
        KeyGrey,
        KeyGreen,

        //Items for PlatformActivation
        WFFirstElevator,
        WFSkeletonPlatform,
        WFSecondElevator,
        MarketElevator,
        MountainsElevator1,
        MountainsElevator2,

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
        Food06,
        Food07,
        Food08,
        Food09,
        Food10,
        Food11,
        Food12,
        Food13,
        Food14,
        Food15,
        Food16,
        Food17,
        Food18,
        Food19,

        Ore00,
        Ore01,
        Ore02,
        Ore03,
        Ore04,
        Ore05,
        Ore06,
        Ore07,
        Ore08,
        Ore09,
        Ore10,
        Ore11
    }
}

