﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI bulletText;

    public Image menuImg;
    public Image centerPoint;
    public GameObject gameUI;
    private bool isMenuOn = false;

    public SliceController mySController;
    
    private void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOn = !isMenuOn;
            menuImg.gameObject.SetActive(isMenuOn);
            gameUI.SetActive(!isMenuOn);
            centerPoint.gameObject.SetActive(!isMenuOn);
            Cursor.visible = isMenuOn;
            mySController.enabled = !isMenuOn;
        }
    }

    public void QuitGame()
    {
        Debug.Log("꺼짐");
        Application.Quit();
    }

    public void ResumeGame()
    {
        
        menuImg.gameObject.SetActive(false);
        centerPoint.gameObject.SetActive(true);
        Cursor.visible = false;
        gameUI.SetActive(true);
        isMenuOn = false;
        mySController.enabled = true;

    }
}
