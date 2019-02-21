using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static UIMgr Instance = null;

    [SerializeField]
    List<GameObject> viewPrefabs = new List<GameObject>();

    [SerializeField]
    Transform uiRoot;

    //public GameObject mainView;
    
    public Dictionary<string/*视图prefab对象名*/, ViewCtrlBase> _views = new Dictionary<string, ViewCtrlBase>();

    private void Awake()
    {
        Instance = this;        
    }

    // Use this for initialization
    void Start ()
    {
        // 初始化所有视图
        foreach (var prefab in viewPrefabs)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.SetParent(uiRoot);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            go.SetActive(false);
            ViewCtrlBase view = go.GetComponent<ViewCtrlBase>();        
            _views.Add(go.name, view);            
        }
        // 开启主视图
        //_views[mainView.name].Show();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
