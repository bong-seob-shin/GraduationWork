using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObj : MonoBehaviour
{
    
    public bool isOn =false;
    [SerializeField] protected bool isSwitch = false;
    public Animation[] interactiveObjAnims;
    public int buttonId = 0;
    
    public static List<InteractObj> buttonList = new List<InteractObj>();
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected  virtual void Update()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            Debug.Log(buttonList[i].name+ " = index" +buttonList[i].buttonId);
        }
    }

    public virtual void InteractObjs()
    {
        
    }
    
   
}
