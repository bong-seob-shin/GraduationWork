using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI bulletText;

    public Image menuImg;
    public GameObject centerPoint;
    public GameObject gameUI;
    [SerializeField]private bool isMenuOn = false;


    public TextMeshProUGUI sensitivityText;
    public Slider sensitivitySlider;
    //public SliceController mySController;
    
    
    public RectTransform crossHair;
    [Range(80f, 250f)]
    public float crossHairSize = 80f;

    private Player _player;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(this.gameObject);
        }
        

    }
    
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    private void Start()
    {
        _player =Player.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOn = !isMenuOn;
            menuImg.gameObject.SetActive(isMenuOn);
            gameUI.SetActive(!isMenuOn);
            centerPoint.SetActive(!isMenuOn);
            
            if (isMenuOn)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            //mySController.enabled = !isMenuOn;
        }


        if (!_player.myGun.gameObject.activeSelf)
        {
            if(centerPoint.activeSelf) centerPoint.SetActive(false);
        }

        _player.sensitivity = sensitivitySlider.value;
        sensitivityText.text = "Sensitivity : "+ (Math.Truncate(sensitivitySlider.value)).ToString();
        crossHair.sizeDelta = new Vector2(crossHairSize , crossHairSize);
    }


    

    public void QuitGame()
    {
        Debug.Log("꺼짐");
        Application.Quit();
    }

    public void ResumeGame()
    {
        
        menuImg.gameObject.SetActive(false);
        centerPoint.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        gameUI.SetActive(true);
        isMenuOn = false;
        //mySController.enabled = true;

    }
}
