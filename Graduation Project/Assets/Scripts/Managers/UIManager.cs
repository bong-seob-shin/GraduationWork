using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI bulletText;

    public Image menuImg;
    public GameObject gameUI;
    private bool isMenuOn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOn = !isMenuOn;
            menuImg.gameObject.SetActive(isMenuOn);
            gameUI.SetActive(!isMenuOn);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        menuImg.gameObject.SetActive(false);
        gameUI.SetActive(true);
        isMenuOn = false;
    }
}
