using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Transform player;
    GameObject blood;
    ViewCtrlMain viewCtrlMain;
    List<AttackType> attackTypeList = new List<AttackType>();
    uint hitVal = 0;

    // Use this for initialization
    void Start () {
        //Time.timeScale = 0;
        //blood = GameObject.Find("BloodPanel");
        viewCtrlMain = GameObject.Find("UI").GetComponentInChildren<ViewCtrlMain>();        
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // 在FixedUpdate捕获键盘会漏，因为按固定时间触发，正确做法应在update里捕获。但作为test可以睁眼闭眼
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            //transform.GetComponent<Animation>().Play("attackJump");
            //Debug.Log(transform.GetComponent<Animation>()["run"].length);
            transform.GetComponent<Animation>()["run"].speed = 2;
            Debug.Log(transform.GetComponent<Animation>()["run"].length);
            transform.GetComponent<Animation>().Play("run");
            //transform.GetComponent<Animation>().CrossFade("run", 2f);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            viewCtrlMain.ShowBlood();
        }
        if (Input.GetKeyUp(KeyCode.O))
        {            
            viewCtrlMain.ShowHit(++hitVal);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            SpriteEffectMgr.Instance.CreateBlood(
                new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + 0.5f, player.transform.localPosition.z),
                new Vector3(90, Random.Range(0, 180), 0));
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            SpriteEffectMgr.Instance.CreateFlowingBlood(
                new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + 0.5f, player.transform.localPosition.z),
                new Vector3(90, Random.Range(0, 180), 0));
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            attackTypeList.Add(AttackType.X);
            viewCtrlMain.ShowCombo(attackTypeList);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            attackTypeList.Add(AttackType.O);
            viewCtrlMain.ShowCombo(attackTypeList);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            attackTypeList.Clear();
            viewCtrlMain.ShowCombo(attackTypeList);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {            
            viewCtrlMain.ShowFullCombo(FullComboType.CLOUD_CUT);
        }
        float time = Time.timeSinceLevelLoad;        
    }

    private void Update()
    {
        int a = 0;
        int b = 0;
        int c = a + b;
        
    }
}
