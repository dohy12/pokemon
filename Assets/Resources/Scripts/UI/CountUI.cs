using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class CountUI : MonoBehaviour
{
    public static CountUI instance;

    private GameObject uiID;
    private SelectUIRedirec redirec;
    private int num;
    private int maxNum;
    private float inputStun = 0;
    private GlobalInput input;
    private TMP_Text txt;
    private UIManager uiManager;

    private void Awake()
    {
        instance = this;
        uiID = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        uiManager = UIManager.instance;
        txt = transform.Find("Text").GetComponent<TMP_Text>();
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
        txt.text = "X " + num.ToString("D2");
    }

    public void Active(int maxNum, SelectUIRedirec redirec, Vector2 pos, params int[] args)
    {
        this.maxNum = maxNum;
        this.redirec = redirec;
        num = 1;

        RectTransform rect = (RectTransform)transform;
        rect.anchoredPosition = pos;
        SetString();
        uiManager.ActiveUI(uiID);
    }

    private void UnActive()
    {
        uiManager.UnActiveUI(uiID);
        RectTransform rect = (RectTransform)transform;
        rect.anchoredPosition = new Vector2(9999f, 9999f);
    }

    private void Redirec(int num)
    {
        redirec.OnSelectRedirec(-1, num);
    }
}
