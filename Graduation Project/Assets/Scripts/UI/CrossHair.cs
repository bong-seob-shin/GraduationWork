using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{

    private RectTransform crossHair;

    [Range(80f, 250f)]
    public float size = 80f;
    // Start is called before the first frame update
    void Start()
    {
        crossHair = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        crossHair.sizeDelta = new Vector2(size , size);
    }
}
