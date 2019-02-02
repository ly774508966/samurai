using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldStatePropValueType
{
	NONE = -1,
	BOOL,
	INT,
	FLOAT,
	VECTOR,
	AGENT,
    EVENT,
    ORDER,
}

public enum WorldStatePropKey
{
	NONE = -1,
    IN_IDLE,
    ORDER,
	AT_TARGET_POS,
    IN_DODGE,
	ALERTED,
	TARGET_ATTACKED,
    LOOKING_AT_TARGET,
	IN_WEAPONS_RANGE,
	WEAPON_IN_HANDS,    
    PLAY_ANIM,
    EVENT,
    IN_BLOCK,
    IN_COMBAT_RANGE,
    AHEAD_OF_ENEMY,
    BEHIND_ENEMY,
    MOVE_TO_RIGHT,
    MOVE_TO_LEFT,
    BOSS_IS_NEAR,
    MOVE_AROUND,
    MAX
}

public enum WorldStateReturnType
{
	INVALID = -1,
	FALSE_RETURN,
	TRUE_RETURN
}

public class Value
{

}

public class ValueVector : Value
{
	public UnityEngine.Vector3 Vector;

	public ValueVector(UnityEngine.Vector3 vector) { Vector = vector; }

    public override string ToString() { return Vector.ToString(); }
}

public class  ValueAgent: Value
{
	public Agent Agent;

	public ValueAgent(Agent a) { Agent = a; }

    public override string ToString() { return Agent.name; }
}

public class ValueBool: Value
{
	public bool Bool;

	public ValueBool(bool b) { Bool = b; }

    public override string ToString() { return Bool.ToString(); }
}

public class ValueFloat : Value
{
	public float Float;

	public ValueFloat(float f) { Float = f; }

    public override string ToString() { return Float.ToString(); }
}

public class ValueInt : Value
{
	public int Int;

	public ValueInt(int i) { Int = i;}

    public override string ToString() { return Int.ToString(); }
}

public class ValueEvent : Value
{
    public EventTypes Event;

    public ValueEvent(EventTypes eventType) { Event = eventType; }

    public override string ToString() { return Event.ToString(); }
}

public class ValueOrder : Value
{
    public OrderType  Order;

    public ValueOrder(OrderType order) { Order = order; }

    public override string ToString() { return Order.ToString(); }
}


[System.Serializable]
public class WorldStateProp
{
	public WorldStatePropKey PropKey { get; set; }
	public string PropName { get { return PropKey.ToString(); } }
	public System.Object PropValue { get; set; }
	public WorldStatePropValueType PropType;
    public float Time;

	public WorldStateProp(bool state) { PropValue = new ValueBool(state); PropType = WorldStatePropValueType.BOOL; }
	public WorldStateProp(int state) { PropValue = new ValueInt(state); PropType = WorldStatePropValueType.INT; }
	public WorldStateProp(float state) { PropValue = new ValueFloat(state); PropType = WorldStatePropValueType.FLOAT; }
	public WorldStateProp(Agent state) { PropValue = new ValueAgent(state); PropType = WorldStatePropValueType.AGENT; }
	public WorldStateProp(UnityEngine.Vector3 vector) { PropValue = new ValueVector(vector); PropType = WorldStatePropValueType.VECTOR; }
    public WorldStateProp(EventTypes eventType) { PropValue = new ValueEvent(eventType); PropType = WorldStatePropValueType.EVENT; }
    public WorldStateProp(OrderType order ) { PropValue = new ValueOrder(order); PropType = WorldStatePropValueType.ORDER; }

	//public static implicit operator WorldStateProp(bool state) { return new WorldStateProp(state);}

	public bool GetBool() { ValueBool b = PropValue as ValueBool; return b != null ? b.Bool : false; }
	public int GetInt() { ValueInt v = PropValue as ValueInt; return v != null ? v.Int : 0; }
	public float GetFloat() { ValueFloat v = PropValue as ValueFloat; return v != null ? v.Float : 0.0f; }
	public UnityEngine.Vector3 GetVector() { ValueVector v = PropValue as ValueVector; return v != null ? v.Vector : Vector3.zero; }
	public Agent GetAgent() { ValueAgent v = PropValue as ValueAgent; return v != null ? v.Agent : null; }
    public EventTypes GetEvent() { ValueEvent v = PropValue as ValueEvent; return v != null ? v.Event : EventTypes.NONE; }
    public OrderType GetOrder() { ValueOrder v = PropValue as ValueOrder; return v != null ? v.Order : OrderType.NONE; }

	public override bool Equals(System.Object o)
	{
		WorldStateProp otherProp = o as WorldStateProp;
		if (otherProp != null)
		{
			if (this.PropType != otherProp.PropType)
				return false;

			switch (this.PropType)
			{
			case WorldStatePropValueType.BOOL:
				return (this.PropValue as ValueBool).Bool == (otherProp.PropValue as ValueBool).Bool;
			case WorldStatePropValueType.INT:
				return (this.PropValue as ValueInt).Int == (otherProp.PropValue as ValueInt).Int;
			case WorldStatePropValueType.FLOAT:
				return (this.PropValue as ValueFloat).Float == (otherProp.PropValue as ValueFloat).Float;
			case WorldStatePropValueType.VECTOR:
				return (this.PropValue as ValueVector).Vector == (otherProp.PropValue as ValueVector).Vector;
			case WorldStatePropValueType.AGENT:
				return (this.PropValue as ValueAgent).Agent == (otherProp.PropValue as ValueAgent).Agent;
			case WorldStatePropValueType.EVENT:
				return (this.PropValue as ValueEvent).Event == (otherProp.PropValue as ValueEvent).Event;
            case WorldStatePropValueType.ORDER:
                return (this.PropValue as ValueOrder).Order == (otherProp.PropValue as ValueOrder).Order;
            default:
				return false;
			}
		}

		return false;
	}

	public override int GetHashCode()
	{
		return (this as object).GetHashCode();
	}

	static public bool operator == (WorldStateProp prop, WorldStateProp other)
	{
		if ((prop as object) == null)
			return (other as object) == null;

		return prop.Equals(other as object);
	}

	static public bool operator != (WorldStateProp prop, WorldStateProp other)
	{
		return !(prop == other);
	}

    public override string ToString()
    {
        return PropName + ": " + PropValue.ToString();
    }

}
[System.Serializable]
public class WorldState
{
	WorldStateProp[] _propState = new WorldStateProp[(int)WorldStatePropKey.MAX];
	BitArray _propBitSet = new BitArray((int)WorldStatePropKey.MAX);

	public WorldStateProp GetWSProperty(WorldStatePropKey key) { return _propState[(int)key]; }
	public bool IsWSPropertySet(WorldStatePropKey key) { return _propBitSet.Get((int)key); }
	public WorldStatePropValueType GetWSPropertyType(WorldStatePropValueType key) { return WorldStatePropValueType.BOOL; } // only bool now

	public void SetWSProperty(WorldStatePropKey key, bool value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

		_propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}

	public void SetWSProperty(WorldStatePropKey key, float value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

		_propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}

	public void SetWSProperty(WorldStatePropKey key, int value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

		_propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}

	public void SetWSProperty(WorldStatePropKey key, Agent value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

		_propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}

	public void SetWSProperty(WorldStatePropKey key, UnityEngine.Vector3 value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

		_propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}

	public void SetWSProperty(WorldStatePropKey key, EventTypes value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

        _propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}

    public void SetWSProperty(WorldStatePropKey key, OrderType value)
	{
        int index = (int)key;
		if (_propState[index] != null)
			WorldStatePropFactory.Collect(_propState[index]);

        _propState[index] = WorldStatePropFactory.Get(key, value);
		_propBitSet.Set(index, true);
	}    

    public void SetWSProperty(WorldStateProp other)
    {
        if (other == null)
            return;

        switch (other.PropType)
        {
            case WorldStatePropValueType.BOOL:
                SetWSProperty(other.PropKey, other.GetBool());
                break;
            case WorldStatePropValueType.INT:
                SetWSProperty(other.PropKey, other.GetInt());
                break;
            case WorldStatePropValueType.FLOAT:
                SetWSProperty(other.PropKey, other.GetFloat());
                break;
            case WorldStatePropValueType.VECTOR:
                SetWSProperty(other.PropKey, other.GetVector());
                break;
            case WorldStatePropValueType.AGENT:
                SetWSProperty(other.PropKey, other.GetAgent());
                break;
            case WorldStatePropValueType.EVENT:
                SetWSProperty(other.PropKey, other.GetEvent());
                break;
            case WorldStatePropValueType.ORDER:
                SetWSProperty(other.PropKey, other.GetEvent());
                break;
            default:
                Debug.LogError("error in SetWSProperty " + other.PropKey.ToString());
                break;
        }
    }

    public void ResetWSProperty(WorldStatePropKey key)
    {
        //Debug.Log("Reset WS property " + key.ToString());
        int i = (int)key;
        if (_propState[i] != null)
        {
            WorldStatePropFactory.Collect(_propState[i]);
            _propState[i] = null;
            _propBitSet.Set(i, false);
        }
    }

	public void Reset() 
	{
		//Debug.Log("Worldstate reset");

		for (int i = 0 ; i < (int)WorldStatePropKey.MAX ; i++)
		{
			if (_propState[i] != null)
			{
				WorldStatePropFactory.Collect(_propState[i]);
				_propState[i] = null;
			}
		}

		_propBitSet.SetAll(false);
	}

	public void Clone(WorldState otherState)
	{
		Reset();
		for (WorldStatePropKey i = 0 ; i < WorldStatePropKey.MAX ; i++)
		{
			if (otherState.GetPropBitSet().Get((int)i) == true)	
				SetWSProperty(otherState.GetWSProperty(i)); 
		}
	}

	public int GetDiffCount(WorldState otherState)
	{	
		int count = 0;
        for (WorldStatePropKey i = 0; i < WorldStatePropKey.MAX; ++i)
        {
			if (otherState.IsWSPropertySet(i) && IsWSPropertySet(i))
			{
				if (GetWSProperty((WorldStatePropKey)i) != otherState.GetWSProperty((WorldStatePropKey)i))
					++count;
			}
			else if (otherState.IsWSPropertySet(i) || IsWSPropertySet(i))
				++count;
		}	
		return count;
	}

    // 以己方为参照，获得对方不满足己方的属性数量
	public int GetUnsatisfiedCount(WorldState otherState)
	{	
		int count = 0;
		for(WorldStatePropKey i = 0; i < WorldStatePropKey.MAX; ++i)
		{
			if (IsWSPropertySet(i) == false)
				continue;
			
			if (otherState.IsWSPropertySet(i) == false)
				++count;

			if (GetWSProperty(i) != otherState.GetWSProperty(i))
				++count;
		}
		return count;
	}

    public bool IsSatisfied(WorldState goal)
    {
        return goal.GetUnsatisfiedCount(this) == 0;
    }

	public BitArray GetPropBitSet()
    {
        return _propBitSet;
    }

    public override string ToString()
    {
        string s = "World state : ";

        for (WorldStatePropKey i = WorldStatePropKey.ORDER; i < WorldStatePropKey.MAX; i++)
        {
            if (IsWSPropertySet(i))
                s += " " + GetWSProperty(i).ToString();
        }

        return s;
    }
}
