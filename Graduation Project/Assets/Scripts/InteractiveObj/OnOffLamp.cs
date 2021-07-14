using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffLamp : MonoBehaviour
{
    public bool isOnOff = false;

    public Material lampOnMat;
    public Material lampOffMat;
    private Renderer _myRenderer;
    private bool _currentOnOff = false;
    // Start is called before the first frame update
    void Start()
    {
        _myRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnOff)
        {
            if (_currentOnOff)
            {
                _myRenderer.material = lampOnMat;
                _currentOnOff = false;
            }
        }
        else
        {
            if (!_currentOnOff)
            {
                _myRenderer.material = lampOffMat;
                _currentOnOff = true;
            }
        }

    }
}
