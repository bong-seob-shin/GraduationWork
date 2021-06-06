using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputIp : MonoBehaviour
{
    public Button connectButton;

    public InputField ipInput;

    private String ip;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onConnectBtnClick()
    {
        ip = ipInput.text;

        SceneManager.LoadScene(1);
        Debug.Log(ip);
    }
}

