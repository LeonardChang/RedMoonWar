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
    /// <summary>
    /// 自己
    /// </summary>
    Self = 0,
    /// <summary>
    /// 自己团队
    /// </summary>
    Team = 1,
    /// <summary>
    /// 敌人
    /// </summary>
    Ememy = 2,
    /// <summary>
    /// 所有人
    /// </summary>
    All = 3,
    /// <summary>
    /// 不需要目标
    /// </summary>
    None = 4,

    Max,
}

/// <summary>
/// 角色势力
/// </summary>
public enum PhaseType : int
{
    /// <summary>
    /// 玩家势力
    /// </summary>
    Charactor = 0,
    /// <summary>
    /// 敌人势力
    /// </summary>
    Enemy,
    /// <summary>
    /// 无势力
    /// </summary>
    None,

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
    /// <summary>
    /// NPC型，随机闲逛，不攻击敌人，不追击仇恨对象
    /// </summary>
    NPC = 0,
    /// <summary>
    /// 迟钝型，随机闲逛，概率攻击敌人，不追击仇恨对象
    /// </summary>
    Retarded,
    /// <summary>
    /// 史莱姆型，随机闲逛，攻击敌人，不追击仇恨对象
    /// </summary>
    Slime,
    /// <summary>
    /// 哥布林型，随机闲逛，攻击敌人，追击仇恨对象
    /// </summary>
    Goblin,
    /// <summary>
    /// 柱子型，原地不动，不攻击敌人，不追击仇恨对象
    /// </summary>
    Pillar,
    /// <summary>
    /// 炮台型，原地不动，攻击敌人，不追击仇恨对象
    /// </summary>
    Cannon,
    /// <summary>
    /// 守卫型，原地不动，攻击敌人，追击仇恨对象
    /// </summary>
    Guard,
    /// <summary>
    /// 杀人狂型，主动接近，攻击敌人，不追击仇恨对象
    /// </summary>
    Assailant,
    /// <summary>
    /// 领袖型，主动接近，攻击敌人，追击仇恨对象
    /// </summary>
    Leader,

    Max,
}

/// <summary>
/// 搜索目标的优先级
/// </summary>
public enum FindTargetConditionType : int
{
    /// <summary>
    /// 随机
    /// </summary>
    Random = 0,
    /// <summary>
    /// 只搜索伤过血的人
    /// </summary>
    BeingHurt = 1,
    /// <summary>
    /// 低血量优先
    /// </summary>
    LowHP = 2, 
    /// <summary>
    /// 高血量优先
    /// </summary>
    HighHP = 3,
    /// <summary>
    /// 属性克制优先
    /// </summary>
    DiElement = 4,
    
    /// <summary>
    /// 只搜索有异常状态的人
    /// </summary>
    HasDebuff = 5,
    /// <summary>
    /// 没有Buff的优先
    /// </summary>
    NoBuff = 6,
    /// <summary>
    /// MP不满的优先
    /// </summary>
    LowMP = 7,

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

public enum SpecialSkillID : int
{
    HealDebuff1 = 67, // 解除1名同伴所有的不良状态
    HealDebuff2 = 68, // 解除周围同伴所有的不良状态
    SexDream = 69, // 减少敌人30%的HP
    HealToMax = 70, // 回满一名同伴所有的HP和MP
    AttackAll1 = 77, // 全屏攻击所有敌人
    AttackAll2 = 78, // 全屏攻击所有敌人
    Dead1 = 79, // 小概率造成敌人即死
    Dead2 = 80, // 中概率造成敌人即死
    Dead3 = 81, // 大概率造成敌人即死
}