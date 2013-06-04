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
    Arrow = 1,          // 普通射箭
    Magic = 2,          // 普通魔法

    FireBall = 3,       // 火魔法
    IceBall = 4,        // 冰魔法
    WindBall = 5,       // 风魔法
    EarthBall = 6,      // 地魔法
    LightBall = 7,      // 光魔法
    DarkBall = 8,       // 暗魔法

    FireArrow = 9,      // 火箭
    IceArrow = 10,      // 冰箭
    WindArrow = 11,     // 风箭
    EarthArrow = 12,    // 地箭
    LightArrow = 13,    // 光箭
    DarkArrow = 14,     // 暗箭

    FireAttak = 15,     // 火攻击
    IceAttak = 16,      // 冰攻击
    WindAttak = 17,     // 风攻击
    EarthAttak = 18,    // 地攻击
    LightAttak = 19,    // 光攻击
    DarkAttak = 20,     // 暗攻击
    
    DefDown = 21,       // 破甲
    SkillDown = 22,     // 沉默
    HitRateDown = 23,   // 致盲
    Sleep = 24,         // 睡眠
    Dizziness = 25,     // 眩晕
    Injuries = 26,      // 重伤
    Poisoning = 27,     // 中毒
   
    AtkUp = 28,         // 斗志
    DefUp = 29,         // 守护
    SpdUp = 30,         // 迅捷
    HPUp = 31,          // 治疗
    MPUp = 32,          // 冥想
    GodHeal = 33,       // 圣佑

    Death = 34,         // 即死
    AllEnemy = 35,      // 全屏攻击

    HPHealth = 36,      // 回血
    MPHealth = 37,      // 回魔
    BuffClear = 38,     // 清Buff

    None,               // 空
    Max,
}

/// <summary>
/// 目标类型
/// </summary>
public enum AttackTargetType : int
{
    Self = 0,   // 自己
    Team = 1,   // 自己团队
    Ememy = 2,  // 敌人
    All = 3,    // 所有人
    None = 4,   // 不需要目标

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
    Water = 0,  // 水
    Fire = 1,   // 火
    Wind = 2,   // 风
    Earth = 3,  // 地
    Light = 4,  // 光
    Dark = 5,   // 暗
    
    None = 6,

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
    Random = 0,     // 随机
    BeingHurt = 1,  // 只搜索伤过血的人
    LowHP = 2,      // 低血量优先
    HighHP = 3,     // 高血量优先
    DiElement = 4,  // 属性克制优先
    
    HasDebuff = 5,  // 只搜索有异常状态的人
    NoBuff = 6,     // 没有Buff的优先
    LowMP = 7,      // MP不满的优先

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

/// <summary>
/// Buff类型
/// </summary>
public enum BuffType : int
{
    Bad = 0,
    Good = 1,
    
    Max,
}

public enum BuffEnum : int 
{
    BrokeDef = 1,
    DisableSkill,
    CanMiss,
    Sleep,
    Dizziness,
    GreatDamage,
    Poison,
    AddAtk,
    AddDef,
    AddSpd,
    AddHPPerround,
    AddMPPerround,
    AddAllPerround,
}

public enum StageSpecialActivity : int 
{
    None = 0,
    DoubleDrop,
    DoubleCoin,
    HalfEnergy,

    Max,
}

public enum SpecialLeaderSkillID : int
{
    BuyPrice1 = 34,
    BuyPrice2 = 35,
    BuyPrice3 = 36,

    NoDebuff = 63,

    CantDie1 = 65,
    CantDie2 = 66,

    GodHP = 67,

    CancelRestraint = 68,
}