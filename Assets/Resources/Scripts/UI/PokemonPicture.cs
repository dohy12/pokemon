using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonPicture : SlideUI
{
    public static PokemonPicture instance;
    private GlobalInput input;
    private GameObject uiID;
    private Image img;

    private float inputStun = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        uiID = gameObject;
        img = transform.Find("Image").GetComponent<Image>();
        SlideUiInit();
    }

    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
        InputUpdate();
    }

    public void Active(int pokeID)
    {
        SlideUiActive();
        UIManager.instance.ActiveUI(uiID);
        SetSprite(pokeID);
        inputStun = 0.4f;
    }

    void UnActive()
    {
        SlideUiUnActive();
        UIManager.instance.UnActiveUI(uiID);
        input.InputStun();
        EventManager.instance.ActiveNextEvent();
    }

    void InputUpdate()
    {
        if (UIManager.instance.CheckUITYPE(uiID))
        {
            inputStun -= Time.deltaTime;
            if ((input.aButtonDown || input.bButtonDown) && inputStun < 0)
            {
                UnActive();
            }
        }        
    }


    void SetSprite(int pokeID)
    {
        var sprite = PokeSpr.instance.sprites[pokeID];
        img.sprite = sprite;
    }
}
