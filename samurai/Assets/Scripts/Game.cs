using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public List<Agent> agents;
    BGM _bgm;
    GameState _gameState;

	// Use this for initialization
	void Awake ()
    {
        Instance = this;
        _bgm = GetComponent<BGM>();
        _gameState = GameState.Game;
    }

    void Start()
    {
        _bgm.FadeIn(_bgm.bgmClip);
    }

    // Update is called once per frame
    void Update ()
    {
        if (IsPause())
        {
            return;
        }
        foreach (var agent in agents)
        {            
            if (agent.gameObject.activeSelf)
            {
                agent.Loop();
            }            
        }
	}

    public void PauseGame()
    {
        _gameState = GameState.Pause;
    }

    public void ResumeGame()
    {
        _gameState = GameState.Game;
    }

    public bool IsPause()
    {
        return _gameState == GameState.Pause;
    }
}
