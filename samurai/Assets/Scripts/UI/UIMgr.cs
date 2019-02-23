using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewType
{
    NONE = 0,
    MAIN,
    OPT,
}

[System.Serializable]
public class ViewData
{
    public ViewType viewType;      // 视图类型
    public GameObject prefab;      // 视图prefab
}

public class UIMgr : MonoBehaviour
{    
    public static UIMgr Instance = null;

    [SerializeField]
    List<ViewData> viewDataList = new List<ViewData>();

    [SerializeField]
    Transform uiRoot;

    //public GameObject mainView;
    
    public Dictionary<ViewType, ViewCtrlBase> views = new Dictionary<ViewType, ViewCtrlBase>();

    private void Awake()
    {
        Instance = this;        
    }

    // Use this for initialization
    void Start ()
    {
        // 初始化所有视图
        foreach (var viewData in viewDataList)
        {
            GameObject go = GameObject.Instantiate(viewData.prefab);
            go.transform.SetParent(uiRoot, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            //go.transform.localRotation = Quaternion.identity;
            go.SetActive(false);
            ViewCtrlBase view = go.GetComponent<ViewCtrlBase>();        
            views.Add(viewData.viewType, view);            
        }
        // 开启主视图
        views[ViewType.MAIN].Show();
	}

}
