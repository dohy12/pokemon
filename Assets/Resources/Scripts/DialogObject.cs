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
            DialogManager.instance.Active(dialogID, null, DialogManager.Type.NORMAL);
        }
        else
        {
            EventManager evm = EventManager.instance;
            switch (eventID)
            {
                case 1://포켓볼
                    if (evm.eventProgress["mainEvent"] == 2)
                    {
                        var pokeballNum = Int32.Parse((transform.name).Substring(8, 1));
                        evm.StartEvent(100, pokeballNum);
                        evm.eventObj = gameObject;
                    }
                    else
                    {
                        DialogManager.instance.Active(dialogID, null, DialogManager.Type.NORMAL);
                    }
                    break;

                case 2://연구소 힐링머신
                    evm.StartEvent(200, 0);
                    break;

                case 3://테스트
                    evm.StartEvent(999, 99999);
                    break;
            }
        }
    }
}
