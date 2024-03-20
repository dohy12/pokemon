using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : SlideUI
{
    public static Bag instance;
    private GlobalInput input;
    private UIManager.TYPE uiType = UIManager.TYPE.BAG;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SlideUiInit();
        input = GlobalInput.globalInput;
    }


    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
        InputCheck();
    }


    public void Active()
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiType);
    }

    public void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI();
    }

    private void InputCheck()
    {
        if (UIManager.instance.CheckUITYPE(uiType))
        {
            if (input.bButtonDown)
            {
                UnActive();
            }
        }
    }
}
