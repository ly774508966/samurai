using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {   

	// Use this for initialization
	void Start () {
        //Time.timeScale = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            //transform.GetComponent<Animation>().Play("attackJump");
            //Debug.Log(transform.GetComponent<Animation>()["run"].length);
            transform.GetComponent<Animation>()["run"].speed = 2;
            Debug.Log(transform.GetComponent<Animation>()["run"].length);
            transform.GetComponent<Animation>().Play("run");
            //transform.GetComponent<Animation>().CrossFade("run", 2f);
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
