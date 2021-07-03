using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [SerializeField]
    private int buttonNum;
    // Start is called before the first frame update


    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < InteractObj.buttonList.Count; i++)
        {
            if (InteractObj.buttonList[i].buttonId == buttonNum)
            {
                InteractObj.buttonList[i].InteractObjs();
                buttonNum = 0;
            }
        }    
    }


    public void SetButton(int key)
    {
        buttonNum = key;
    }
}
