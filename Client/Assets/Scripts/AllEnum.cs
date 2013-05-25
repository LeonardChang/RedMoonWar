/// <summary>
/// 升级类型
/// 不同的类型采用不同的升级经验曲线
/// </summary>
public enum GrowType : int
{
    TypeA = 0,
    TypeB,
    TypeC,

    Max,
}

/// <summary>
/// 攻击动画效果类型
/// </summary>
public enum AttackAnimType : int
{
    NormalAttack = 0,   // 普通攻击
    Arrow = 1,          // 射箭
    FireBall = 2,       // 火球
    IceBall = 3,        // 冰球
    WindBall = 4,       // 风攻击
    StoneBall = 5,      // 地攻击
    LightBall = 6,      // 光攻击
    DarkBall = 7,       // 暗攻击
    CannonBall = 8,     // 炮击

    HPHealth = 9,       // 回血
    MPHealth = 10,      // 回魔

    None,               // 空
    Max,
}

/// <summary>
/// 目标类型
/// </summary>
public enum AttackTargetType : int
{
    Self = 0,   // 自己
    Team,       // 自己团队
    Ememy,      // 敌人
    All,        // 所有人

    Max,
}

/// <summary>
/// 角色势力
/// </summary>
public enum PhaseType : int
{
    Charactor = 0,  // 玩家势力
    Enemy,          // 敌人势力
    None,           // 无势力

    Max,
}

/// <summary>
/// 属性类型
/// </summary>
public enum ElementType : int
{
    Fire = 0,   // 火
    Water = 1,  // 水
    Wind = 2,   // 风
    Earth = 3,  // 地
    Light = 4,  // 光
    Dark = 5,   // 暗

    Max,
}

/// <summary>
/// 敌人的AI类型
/// </summary>
public enum AIType : int
{
    NPC = 0,    // 随机闲逛，不攻击范围内的敌人，不追击仇恨对象
    Retarded,   // 随机闲逛，概率攻击范围内的敌人，不追击仇恨对象
    Slime,      // 随机闲逛，攻击范围内的敌人，不追击仇恨对象
    Goblin,     // 随机闲逛，攻击范围内的敌人，追击仇恨对象
    Pillar,     // 原地不动，不攻击范围内的敌人，不追击仇恨对象
    Cannon,     // 原地不动，攻击范围内的敌人，不追击仇恨对象
    Guard,      // 原地不动，攻击范围内的敌人，追击仇恨对象
    Assailant,  // 主动接近，攻击范围内的敌人，不追击仇恨对象
    Leader,     // 主动接近，攻击范围内的敌人，追击仇恨对象

    Max,
}

/// <summary>
/// 搜索目标的优先级
/// </summary>
public enum FindTargetConditionType : int
{
    Random = 0, // 随机
    BeingHurt,  // 只搜索伤过血的人
    LowHP,      // 低血量优先
    HighHP,     // 高血量优先
    DiElement,  // 属性克制优先

    Max,
}

/// <summary>
/// 掉落道具的类型
/// </summary>
public enum DropRewardType : int
{
    None = 0,
    Coin,
    Card,
    
    Max,
}

/// <summary>
/// 卡牌排序方式
/// </summary>
public enum CharacterSequenceType : int
{
    Element = 0,
    HP,
    ATK,
    DEF,
    SPD,
    GetTime,

    Max,
}

/// <summary>
/// 地图道具种类
/// </summary>
public enum MapItemType : int
{
    None = 0,

    HPFood1,
    HPFood2,
    HPFood3,
    MPFood1,
    MPFood2,
    MPFood3,

    Chest1,
    Chest2,
    Chest3,

    Max,
}