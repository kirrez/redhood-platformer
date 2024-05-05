namespace Platformer
{
    public enum EQuest
    {
        Stage,
        Location,
        SpawnPoint,
        Confiner,

        //Mother's Pie quest
        MotherPie,
        MushroomsCollected,
        MushroomsRequired,
        BlackberriesCollected,
        BlackberriesRequired,
        //

        SuspensionBridge, //Suspension bridge quest
        Drawbridge,
        UpgradeHealth, // repeating quest, giving health upgrades infinitely
        Blacksmith, // giving GREY key for 3 ore
        PlatformSuspended, //quest in mountains
        WomanCandy, // secret candy in village
        Hermit, // tells about way to forsaken church
        BrokenLeverInCaves, // chain for opening door to abandoned chapel in cave labyrinth
        BrokenLeverTip, // tells us about found item
        VengefulSpiritDisturbed, // spirit appears right after acquairing of Weak Holy Water
        AssistantMegafrog, // assists with lowering Megafrog's HP for money. In future will work on EASY level.
        AssistantTower, // assists with tower in WesternForest for money, mentioned to be used on EASY level.
        Guide_v01, // character with explanation in v 0.1
        StartGameWeaponsReset, // configurates weapons right after tutorial in "TheVillage" stage

        SwitchHandleItem, // item accessible after defeating Megafrog, for broken lever repair
        CrippledAxeItem, // appears as collectible item in Western Forest
        WeakHolyWaterItem, // appears in Abandoned Chapel in Cave Labyrinth
        SharpenedAxeItem, // appears in Mountain Pass in v 0.1
        FarmerKnifeItem, // appears in Mountain Pass in v 0.1
        KitchenKnifeItem, // tutorial
        TutorialAxeItem, // tutorial
        TutorialCheeseItem, // tutorial

        //Bosses
        Megafrog, //0- battle available, 1- battle won
        MegafrogMaxHealth,

        KnifeLevel, // 0-none, 1-KitchenKnife, 2-FarmerKnife, 3-HunterKnife
        AxeLevel, // 0-none, 1-CrippledAxe(no WoodBreaker), 2-SharpenedAxe (+WB), 3-SturdyAxe
        HolyWaterLevel, // 0-none, 1-WeakHolyWater(Undead Stun only), 2-StrongHolyWater(+ normal damage) 3 - Cheese Bomb (tutorial in v 0.1 and real in v 1.0)

        FarmerKnifeCost,
        HunterKnifeCost,
        SharpenedAxeCost, //in ore ???
        SturdyAxeCost,
        StrongHolyWaterCost,

        //Player's max lives (3-8)
        MaxLives,    //Health
        MaxLivesCap, //Health
        Money,
        TriesLeft,   //Lives
        GameMode,    //Determins implementation of Lives concept. Easy mode - infinite lives, Normal mode - limited amount

        LifeUpgradeCost,
        FoodCollected,
        OreCollected,

        //Collectibles, set of each type must go one after another ))
        KeyRed,
        KeyGrey,
        KeyGreen,

        //Items for PlatformActivation
        WFFirstElevator, // index 0
        WFSkeletonPlatform,
        WFSecondElevator,
        MarketElevator,
        MountainsElevator1,
        MountainsElevator2,
        Cave1Platform,
        Cave6Platform,
        KeyCave3Platform,

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
        Ore11,
        Ore12,
        Ore13,
        Ore14,
        Ore15,
        Ore16,
        Ore17,
        Ore18,
        Ore19,

        Replenish00,
        Replenish01,
        Replenish02,
        Replenish03,
        Replenish04,
        Replenish05,
        Replenish06,
        Replenish07,
        Replenish08,
        Replenish09,

        //Money Items
        Coin50_00,
        Coin50_01,
        Coin50_02,
        Coin50_03,
        Coin50_04,
        Coin50_05,
        Coin50_06,
        Coin50_07,
        Coin50_08,
        Coin50_09,
        Coin50_10,
        Coin50_11,
        Coin50_12,
        Coin50_13,
        Coin50_14,
        Coin50_15,
        Coin50_16,
        Coin50_17,
        Coin50_18,
        Coin50_19,
        Coin50_20,
        Coin50_21,
        Coin50_22,
        Coin50_23,
        Coin50_24,
        Coin50_25,
        Coin50_26,
        Coin50_27,
        Coin50_28,
        Coin50_29,
        Coin50_30,
        Coin50_31,
        Coin50_32,
        Coin50_33,
        Coin50_34,
        Coin50_35,
        Coin50_36,
        Coin50_37,
        Coin50_38,
        Coin50_39,
        Coin50_40,
        Coin50_41,
        Coin50_42,
        Coin50_43,
        Coin50_44,
        Coin50_45,
        Coin50_46,
        Coin50_47,
        Coin50_48,
        Coin50_49,
        Coin50_50,
        Coin50_51,
        Coin50_52,
        Coin50_53,
        Coin50_54,
        Coin50_55,
        Coin50_56,
        Coin50_57,
        Coin50_58,
        Coin50_59,
        Coin50_60,

        Bag250_00,
        Bag250_01,
        Bag250_02,
        Bag250_03,
        Bag250_04,
        Bag250_05,
        Bag250_06,
        Bag250_07,
        Bag250_08,
        Bag250_09,

        Bag600_00,
        Bag600_01,
        Bag600_02,
        Bag600_03,
        Bag600_04,
        Bag600_05,
        Bag600_06,
        Bag600_07,
        Bag600_08,
        Bag600_09,

        Bag1500_00,
        Bag1500_01,
        Bag1500_02,
        Bag1500_03,
        Bag1500_04,
        Bag1500_05,
        Bag1500_06,
        Bag1500_07,
        Bag1500_08,
        Bag1500_09,
    }
}