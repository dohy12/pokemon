using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money instance;
    private int money = 10000;
    public Vector2 startPos;
    public Vector2 endPos;
    private RectTransform rectT;
    private TMP_Text txt;


    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    public void Active()
    {
        rectT.anchoredPosition = endPos;
        ShowMoney();
    }

    public void UnActive()
    {
        rectT.anchoredPosition = startPos;
    }

    public void Init()
    {
        rectT = (RectTransform)transform;
        rectT.anchoredPosition = startPos;
        txt = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void ShowMoney() { txt.text = money.ToString()  + " ¿ø"; }

    public void SetMoney(int money)
    { 
        this.money = money;
        ShowMoney();
    }

    public int GetMoney() {  return this.money; }
}
