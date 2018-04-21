using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDungeon : MonoBehaviour {
    public GameObject introMsg;
    public GameObject fireMsg;
    public GameObject fireMsg2;
    public GameObject keyMsg;
    private bool flag = false, flag2 = false, flag3 = false;
	// Use this for initialization
	void Start () {
        if (GameSaver.liveSave.firstFire) introMsg.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameSaver.liveSave.firstFire && !flag) OpenMsg();
        if (GameSaver.liveSave.firstFire && Input.GetKeyDown("1") && !flag2) OpenMsg2();
        if (GameSaver.liveSave.firstKey && !flag3) KeyMsg();
    }

    void OpenMsg()
    {
        fireMsg.SetActive(true);
        flag = true;
    }

    void OpenMsg2()
    {
        fireMsg.SetActive(false);
        fireMsg2.SetActive(true);
        flag2 = true;
    }

    void KeyMsg()
    {
        keyMsg.SetActive(true);
        flag3 = true;
    }
}
