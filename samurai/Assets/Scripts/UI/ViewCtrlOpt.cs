using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlOpt : ViewCtrlBase
{
    public Button backBtn;
    public Button helpBtn;
    public Button exitBtn;

	// Use this for initialization
	void Start ()
    {
        backBtn.onClick.AddListener(OnBackBtnClick);
        helpBtn.onClick.AddListener(OnHelpBtnClick);
        exitBtn.onClick.AddListener(OnExitBtnClick);
    }

    void OnBackBtnClick()
    {
        Game.Instance.ResumeGame();
        Time.timeScale = 1;
        Hide();
        UIMgr.Instance.views[ViewType.MAIN].Show();
    }

    void OnHelpBtnClick()
    {

    }

    void OnExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif        
    }
}
