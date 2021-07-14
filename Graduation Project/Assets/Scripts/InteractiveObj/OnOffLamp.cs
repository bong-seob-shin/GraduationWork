using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffLamp : MonoBehaviour
{
    public bool isOnOff = false;

    public Material lampOnMat;
    public Material lampOffMat;

    private bool currentOnOff = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnOff)
        {
            if (currentOnOff)
            {
                gameObject.GetComponent<Renderer>().material = lampOnMat;
                currentOnOff = false;
            }
        }
        else
        {
            if (!currentOnOff)
            {
                gameObject.GetComponent<Renderer>().material = lampOffMat;
                currentOnOff = true;
            }
        }

    }
}
