using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlOpt : MonoBehaviour
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

    }

    void OnHelpBtnClick()
    {

    }

    void OnExitBtnClick()
    {

    }
}
