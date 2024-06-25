public enum InventoryLocation
{
    player,
    chest,
    count 
}

public enum ToolEffects
{
    none,
    Watering,
}

public enum Direction 
{
    none,
    up,
    down,
    left,
    right
}

public enum ItemType
{
    Seed,
    Commodity,
    WateringTool,
    HoepingTool,
    ChopingTool,
    BreakingTool,
    ReapingTool,
    CollectingTool,
    ReapableScenary,
    Furniture,
    count,
    none
}
public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
    count,
    none
}

public enum SceneName
{
    Scene1_Farm,
    Scene2_Field,
    Scene3_Cabin,
}
public enum AnimationName
{
    idleDown,
    idleUp,
    idleRight,
    idleLeft,
    walkDown,
    walkUp,
    walkRight,
    walkLeft,
    runUp,
    runRight,
    runLeft,
    runDown,
    useToolUp,
    useToolRight,
    useToolLeft,
    useToolDown,
    swingToolRight,
    swingToolLeft,
    swingToolUp,
    swingToolDown,
    liftToolRight,
    liftToolLeft,
    liftToolUp,
    liftToolDown,
    holdToolRight,
    holdToolLeft,
    holdToolUp,
    holdToolDown,
    pickRight,
    pickLeft,
    pickUp,
    pickDown,
    count,
}
public enum CharacterPartAnimator
{
    Body,
    Arms,
    Hair,
    Tool,
    Hat,
    Count
}

public enum PartVariantColor
{
    none,
    count
}

public enum PartVariantType
{ 
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count

}

public enum GridBoolProperty
{
    diggable,
    canDropItem,
    canPlaceFurniture,
    isPath,
    isNPCObstacle
}
