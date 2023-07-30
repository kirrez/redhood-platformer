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

        Mushroom1,
        Mushroom2,
        Mushroom3,
        Blackberry1,
        Blackberry2,
        Blackberry3,
        Blackberry4,

        SuspensionBridge,
        Drawbridge,
        WFElevator,
        WFPlatform,
        WFSecondElevator,

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

