using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorTrigger : MonoBehaviour
{
    public enum eTrigger
    {
        Lamp,None
    }

    public eTrigger triggerType;

    public GameObject[] triggers;

    private bool isPlayed = false;
    private bool isPuzzleCorrected = false;
    
    public Animation doorAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayed)
        {
            if (triggerType == eTrigger.Lamp)
            {
                foreach (var go in triggers)
                {
                    if (go.GetComponent<OnOffLamp>().isOnOff)
                    {
                        isPuzzleCorrected = true;
                    }
                    else
                    {
                        isPuzzleCorrected = false;
                        break;
                    }
                    
                }
            }


            if (isPuzzleCorrected)
            {
                doorAnim.Play();
                isPlayed = true;
            }
        }
    }
}
