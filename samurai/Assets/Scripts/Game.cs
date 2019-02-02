using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public List<Agent> agents;
    BGM _bgm;

	// Use this for initialization
	void Awake ()
    {
        Instance = this;
        _bgm = GetComponent<BGM>();

    }

    void Start()
    {
        _bgm.FadeIn(_bgm.bgmClip);
    }

    // Update is called once per frame
    void Update ()
    {
        foreach (var agent in agents)
        {            
            if (agent.gameObject.activeSelf)
            {
                agent.Loop();
            }            
        }
	}
}
