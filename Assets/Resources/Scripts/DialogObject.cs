using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogObject : MonoBehaviour
{
    public int dialogID;
    public int objKind = 0;
    public int eventID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveDialog()
    {
        if (objKind == 0)
        {
            DialogManager.instance.Active(dialogID);
        }
        else
        {
            switch (eventID)
            {
                case 1://Æ÷ÄÏº¼
                    var pokeballNum = Int32.Parse((transform.name).Substring(8,1));
                    EventManager.instance.StartEvent(5, pokeballNum);
                    EventManager.instance.eventObj = gameObject;
                    break;
            }
        }
    }
}
