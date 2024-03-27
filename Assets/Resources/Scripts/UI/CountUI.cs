using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class CountUI : MonoBehaviour
{
    public static CountUI instance;
    public static CountUI monCountInstance;

    private GameObject uiID;
    private SelectUIRedirec redirec;
    private int num;
    private int maxNum;
    private int itmID;
    private int redirectID;
    private float inputStun = 0;
    private GlobalInput input;
    private TMP_Text txt;
    private TMP_Text money;
    private UIManager uiManager;

    public bool isShop = false;
    private int shopPrice = 200;

    private void Awake()
    {
        if (!isShop)
        {
            instance = this;
        }
        else
        {
            monCountInstance = this;
        }
        
        uiID = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        uiManager = UIManager.instance;
        txt = transform.Find("Text").GetComponent<TMP_Text>();

        if (isShop)
        {
            money = transform.Find("Money").GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (uiManager.CheckUITYPE(uiID))
        {
            inputStun -= Time.deltaTime;
            if (input.verticalRaw !=0 && inputStun < 0)
            {
                inputStun = 0.2f;
                num += (int)input.verticalRaw;
                if(num < 1){ num = maxNum; }
                if (num > maxNum) { num = 1; }
                SetString();
            }

            if (input.horizontalRaw != 0 && inputStun < 0)
            {
                inputStun = 0.2f;
                num += (int)input.horizontalRaw * 10;
                if (num < 1) { num = 1; }
                if (num > maxNum) { num = maxNum; }
                SetString();
            }

            if (input.aButtonDown)
            {
                UnActive();
                Redirec(num);
            }

            if (input.bButtonDown)
            {
                UnActive();
                Redirec(0);
            }
        }
    }

    private void SetString()
    {
        if (!isShop)
        {
            txt.text = "X " + num.ToString("D2");
        }
        else
        {
            txt.text = "X " + num.ToString("D2");
            money.text = (num * shopPrice).ToString() + "  ¿ø";
        }
        
    }

    public void Active(int maxNum, SelectUIRedirec redirec, Vector2 pos, params int[] args)
    {
        this.maxNum = maxNum;
        this.redirec = redirec;
        num = 1;

        RectTransform rect = (RectTransform)transform;
        rect.anchoredPosition = pos;
        
        uiManager.ActiveUI(uiID);

        itmID = args[0];
        redirectID = args[1];   
        
        if (isShop)
        {
            shopPrice = ItemInfo.instance.info[itmID].price;
        }

        SetString();
    }

    private void UnActive()
    {
        uiManager.UnActiveUI(uiID);
        RectTransform rect = (RectTransform)transform;
        rect.anchoredPosition = new Vector2(9999f, 9999f);
    }

    private void Redirec(int num)
    {
        if (!isShop)
            redirec.OnSelectRedirec(num, redirectID, 0, itmID);
        else
            redirec.OnSelectRedirec(num, redirectID, 1, itmID);
    }
}
