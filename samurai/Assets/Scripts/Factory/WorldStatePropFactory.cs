using UnityEngine;
using System.Collections.Generic;

public static class WorldStatePropFactory
{
	private static Queue<WorldStateProp> m_UnusedProps = new Queue<WorldStateProp>();


	static public WorldStateProp Get(WorldStatePropKey key, bool state)
	{
		WorldStateProp p = null;

		if (m_UnusedProps.Count > 0)
		{
			p = m_UnusedProps.Dequeue();
			p.PropValue = new ValueBool(state);
			p.PropType = WorldStatePropValueType.BOOL;
		}
		else
			p = new WorldStateProp(state);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
		p.PropKey = key;
		return p;
	}

	static public WorldStateProp Get(WorldStatePropKey key, int state)
	{
		WorldStateProp p;

		if (m_UnusedProps.Count > 0)
		{
			p = m_UnusedProps.Dequeue();
			p.PropValue = new ValueInt(state);
			p.PropType = WorldStatePropValueType.INT;
		}
		else
			p = new WorldStateProp(state);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
		p.PropKey = key;
		return p;
	}

	static public WorldStateProp Get(WorldStatePropKey key, float state)
	{
		WorldStateProp p;

		if (m_UnusedProps.Count > 0)
		{
			p = m_UnusedProps.Dequeue();
			p.PropKey = key;
			p.PropValue = new ValueFloat(state);
		}
		else
			p = new WorldStateProp(state);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
		p.PropType = WorldStatePropValueType.FLOAT;
		return p;
	}

	static public WorldStateProp Get(WorldStatePropKey key, Agent state)
	{
		WorldStateProp p = null;

		if (m_UnusedProps.Count > 0)
		{
			p = m_UnusedProps.Dequeue();
			p.PropValue = new ValueAgent(state);
			p.PropType = WorldStatePropValueType.AGENT;
		}
		else
			p = new WorldStateProp(state);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
		p.PropKey = key;
		return p;
	}

	static public WorldStateProp Get(WorldStatePropKey key, UnityEngine.Vector3 vector)
	{
		WorldStateProp p = null;

		if (m_UnusedProps.Count > 0)
		{
			p = m_UnusedProps.Dequeue();
			p.PropValue = new ValueVector(vector);
			p.PropType = WorldStatePropValueType.VECTOR;
		}
		else
			p = new WorldStateProp(vector);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
		p.PropKey = key;
		return p;
	}

    static public WorldStateProp Get(WorldStatePropKey key, EventTypes eventType)
    {
        WorldStateProp p = null;

        if (m_UnusedProps.Count > 0)
        {
            p = m_UnusedProps.Dequeue();
            p.PropValue = new ValueEvent(eventType);
            p.PropType = WorldStatePropValueType.EVENT;
        }
        else
            p = new WorldStateProp(eventType);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
        p.PropKey = key;
        return p;
    }

    static public WorldStateProp Get(WorldStatePropKey key, OrderType orderType)
    {
        WorldStateProp p = null;

        if (m_UnusedProps.Count > 0)
        {
            p = m_UnusedProps.Dequeue();
            p.PropValue = new ValueOrder(orderType);
            p.PropType = WorldStatePropValueType.EVENT;
        }
        else
            p = new WorldStateProp(orderType);

        p.Time = UnityEngine.Time.timeSinceLevelLoad;
        p.PropKey = key;
        return p;
    }
    
	static public void Collect(WorldStateProp prop) 
    {
		prop.PropKey = WorldStatePropKey.NONE;
		m_UnusedProps.Enqueue(prop); 
	}
}
