using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChoppedBodyType
{
    NONE = -1,
    LEGS = 0,
    BEHEADED,
    HALF_BODY,
    SLICE_FRONT_BACK,
    SLICE_LEFT_RIGHT,
    MAX,
}

[System.Serializable]
public class ChoppedBodyInfo
{
    public AgentType agentType; // agent类型
    public List<ChoppedBodyData> data = new List<ChoppedBodyData>();
}

[System.Serializable]
public class ChoppedBodyData
{
    public ChoppedBodyType choppedBodyType; // 尸块类型
    public List<GameObject> prefabs = new List<GameObject>();
}

public class ChoppedBodyFactory : MonoBehaviour
{
   public static ChoppedBodyFactory Instance;

    public List<ChoppedBodyInfo> choppedBodies = new List<ChoppedBodyInfo>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Get(AgentType agentType, Transform trans, ChoppedBodyType choppedBodyType)
    {        
        for (int i = 0; i < choppedBodies.Count; ++i)
        {
            if (choppedBodies[i].agentType == agentType)
            {
                ChoppedBodyInfo cbi = choppedBodies[i];
                for (int ii = 0; ii < cbi.data.Count; ++ii)
                {
                    ChoppedBodyData cbd = cbi.data[ii];
                    if (cbd.choppedBodyType == choppedBodyType)
                    {
                        if (cbd.prefabs.Count == 0)
                        {
                            return null;
                        }

                        GameObject go = cbd.prefabs[cbd.prefabs.Count - 1];
                        if (cbd.prefabs.Count > 1)
                        {                            
                            go.SendMessage("Activate", trans);
                            cbd.prefabs.Remove(go);                            
                        }
                        else if(cbd.prefabs.Count > 0)
                        {
                            // 必须留一个用来Instantiate
                            go = GameObject.Instantiate(go);
                            go.SendMessage("Activate", trans);                            
                        }
                        return go;
                    }                    
                }                
            }
        }       

        return null;
    }

    public void Collect(GameObject gameObject, AgentType agentType, ChoppedBodyType choppedBodyType)
    {
        if (gameObject == null)
        {
            return;
        }

        gameObject.SendMessage("Deactivate");

        for (int i = 0; i < choppedBodies.Count; ++i)
        {
            if (choppedBodies[i].agentType == agentType)
            {
                ChoppedBodyInfo cbi = choppedBodies[i];
                for (int ii = 0; ii < cbi.data.Count; ++ii)
                {
                    ChoppedBodyData cbd = cbi.data[ii];
                    if (cbd.choppedBodyType == choppedBodyType)
                    {
                        cbd.prefabs.Add(gameObject);
                    }
                }
            }
        }
    }
}
