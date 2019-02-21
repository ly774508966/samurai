using UnityEngine;
using System.Collections;


public enum HardwareType
{
    iPhone3G = 0,
    iPhone4G = 1,
    iPad = 2,
    Max = 3,
}

public enum WeaponType 
{
	None = -1,
	Katana = 0,
	Body,
    Bow,
	Max,
}

public enum BlockState
{
    None = -1,
    Start = 0,
    Loop,
    End,
    HitBlocked,
    Failed
}

public enum KnockdownState
{
    None = -1,
    Down = 0,
    Loop,
    Up,
    Fatality,
}

public enum WeaponState
{
    NotInHands,
	Ready,
	//Attacking,
	//Reloading,
	//Empty,
}

public enum AttackType
{
	None = -1,
	X = 0,
	O = 1,
    BossBash = 2,
    Fatality = 3,
    Counter = 4,
    Berserk = 5,
	Max = 6,
}
    
public enum AgentType
{
	NONE = -1,
    SWORD_MAN = 0,
    SWORD_MAN_LOW,
    PEASANT,
    PEASANT_LOW,
    DOUBLE_SWORDS_MAN,
    BOW_MAN,    
    MINI_BOSS01,    
    BOSS_OROCHI,
	NPC_MAX,
    PLAYER
}

public enum GameState
{
	MainMenu,
	IngameMenu,
	Game,
	SaleScreen,
    Tutorial,
    Shop,
}

public enum GameType
{
	SinglePlayer,
    ChapterOnly,
	Survival,
    FirstTimeTutorial,
    Tutorial,
    SaleScreen,
}

public enum GameDifficulty
{
    Easy,
	Normal,
	Hard,
}

public enum DamageType
{
    Front,
    Back,
    BreakBlock, // ÆÆ·À
    InKnockdown,    
}

public enum CriticalHitType
{
    None,
    Vertical,
	Horizontal,
}

public enum ComboLevel
{
	One = 1,
	Two = 2,
    Three = 3,
    Max = 3
}

public enum ComboLevelPrice
{
    One = 0,
    Two = 1000,
    Three = 2000
}

public enum SwordLevel
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Max = 5
}

public enum SwordLevelPrice
{
    One = 0,
    Two = 1000,
    Three = 1500,
    Four = 2000,
    Five = 3000,
}

public enum HealthLevel
{
    One = 1,
    Two = 2,
    Three = 3,
    Max = 3
}

public enum HealtLevelPrice
{
    One = 0,
    Two = 1500,
    Three = 3000,
}

public enum RotationType
{
    LEFT,
    RIGHT
}

public enum MotionType
{
	NONE,
	WALK,
	RUN,
    SPRINT,
    ROLL,
    ATTACK,
    BLOCK,
    //BLOCKING_ATTACK,
    INJURY,    
    DEATH,
    KNOCKDOWN,
}

public enum MoveType
{
    NONE,
    FORWARD,
    BACKWARD,
    LEFTWARD,
    RIGHTWARD,
}

public enum LookType
{
    None,
    TrackTarget,
}


public enum EventTypes
{
    NONE,
    //ENEMYSTEP,
    //ENEMYSEE,
    //ENEMYLOST,
    HIT,
    DEAD,
    //IMINPAIN,
    BLOCK_BROKEN,    // ±»ÈËÆÆ·À
    KNOCKDOWN,
    //FriendInjured,
}

public enum OrderType
{
    NONE = -1,
    GOTO,
    ATTACK,
    DODGE,    
    STOPMOVE,
}

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

public enum BlockResult
{
    NONE = 0,
    SUCCESS,
    FAIL,
}

public enum FullComboType
{
    NONE = 0,
    RAISE_WAVE,         // XXXXO ÀË·­£¨¿ìËÙ£©
    HALF_MOON,          // OOOXX °ëÔÂ£¨ÆÆ·À£©
    CLOUD_CUT,          // OOXXX ÔÆÇÐ£¨ÖÂÃü£©
    WALKING_DEATH,      // XXOXX Ì¤ËÀ£¨»÷µ¹£©
    CRASH_GENERAL,      // OXOOO ÆÆ½«£¨ÖØ»÷¡¢ÈºÉË£©
    FLYING_DRAGON,      // XOOXX ·ÉÁú
}